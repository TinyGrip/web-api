using OutdoorSolution.Dto;
using OutdoorSolution.Dto.Infrastructure;
using Resta.UriTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Http.Routing;

namespace OutdoorSolution.Helpers
{
    public static class UrlHelperExtentions
    {
        /// <summary>
        /// Creates link to specified controller's method
        /// </summary>
        /// <typeparam name="T">Api Controller</typeparam>
        /// <param name="urlHelper"></param>
        /// <param name="expr">Action method to create link for</param>
        /// <returns></returns>
        public static Link Link<T>(this UrlHelper urlHelper, Expression<Func<T, object>> expr)
        {
            // Body expression has to be method in order to read parameters
            MethodCallExpression bodyExpr = expr.Body as MethodCallExpression;
            if (bodyExpr == null)
                throw new Exception("Wrong expression!");

            var urlParamsDictionary = FormUrlParams(bodyExpr);

            // find a route and create a link
            var apiDescription = GetApiDescription<T>(urlHelper.Request, bodyExpr.Method.Name);
            var link = new Link();
            link.Method = apiDescription.ActionDescriptor.SupportedHttpMethods.FirstOrDefault().Method;

            if (apiDescription.Route.RouteTemplate.Contains("{controller}"))
            {
                // add controller name in case we create link to other than current controller
                urlParamsDictionary.Add("controller", typeof(T).Name.Replace("Controller", ""));
            }

            UriTemplate template = new UriTemplate(GetBaseAddress(urlHelper.Request) + apiDescription.Route.RouteTemplate);
            link.Href = template.ResolveUri(urlParamsDictionary);

            // add query parameters which are not in the template
            // this has to be done, because Resta library does not add parameters to url, which are not in route template
            foreach (var p in urlParamsDictionary)
            {
                var variable = template.Variables.FirstOrDefault(v => v.Name == p.Key);
                if (variable == null) // means variable was not used by Resta lib
                {
                    link.Href = link.Href.AddParameter(p.Key, p.Value.ToString());
                }
            }
            
            return link;
        }

        /// <summary>
        /// Retrieves api description for specified controller's action
        /// </summary>
        /// <typeparam name="T">Controller's type</typeparam>
        /// <param name="request"></param>
        /// <param name="actionName">Action name of interest</param>
        /// <returns></returns>
        private static ApiDescription GetApiDescription<T>(HttpRequestMessage request, string actionName)
        {
            var allApiDescriptions = request.GetConfiguration().Services.GetApiExplorer().ApiDescriptions;

            return allApiDescriptions.Where(x => x.ActionDescriptor.ControllerDescriptor.ControllerType == typeof(T) &&
                                                   x.ActionDescriptor.ActionName == actionName)
                                     .OrderBy(x =>
                                     {
                                         double orderValue = 0;
                                         if (x.Route != null && x.Route.DataTokens != null && x.Route.DataTokens.Keys.Contains("order"))
                                             orderValue = Convert.ToDouble(x.Route.DataTokens["order"]);

                                         return orderValue;
                                     })
                                     .ThenBy(x =>
                                     {
                                         double precedence = 0;
                                         if (x.Route != null && x.Route.DataTokens != null && x.Route.DataTokens.Keys.Contains("precedence"))
                                             precedence = Convert.ToDouble(x.Route.DataTokens["precedence"]);
                                         return precedence;
                                     })
                                     .First();
        }

        /// <summary>
        /// Creates url params dictionary, by reading action method call expression on controller's type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        private static Dictionary<string, object> FormUrlParams(MethodCallExpression bodyExpr)
        {
            // dictionary of parameters for controller's action
            var urlParamsDictionary = new Dictionary<string, object>();

            // fill dictionary with provided action parameters
            var methodParams = bodyExpr.Method.GetParameters();
            foreach (var p in methodParams)
            {
                // get parameter's value
                var argDelegate = Expression.Lambda(bodyExpr.Arguments[p.Position]).Compile();
                var currArgument = argDelegate.DynamicInvoke();

                // save name value pair depending on parameter type
                if (IsSystemType(p.ParameterType)) // system type value should be saved as is
                {
                    urlParamsDictionary.Add(p.Name, currArgument.ToString());
                }
                else if (HasFromUriAttr(p)) // in case of user type - property name-values should be extracted 
                {
                    ReadObjectProperties(urlParamsDictionary, p.ParameterType, currArgument);
                }
            }

            return urlParamsDictionary;
        }

        /// <summary>
        /// Saves object property-value pair into passed dictionary
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="type"></param>
        /// <param name="argumentObj"></param>
        private static void ReadObjectProperties(Dictionary<string, object> dictionary, Type type, object argumentObj)
        {
            if (argumentObj == null)
                return;

            foreach (var objProp in type.GetProperties())
            {
                var objPropValue = objProp.GetValue(argumentObj);
                if (objPropValue == null)
                    continue;

                //if (!IsSystemType(objProp.PropertyType))
                //{
                //    ReadObjectProperties(dictionary, objProp.PropertyType, objPropValue);
                //}

                dictionary.Add(objProp.Name, objPropValue.ToString());
            }
        }

        private static bool IsSystemType(Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(DateTime)
                || type == typeof(TimeSpan) || type == typeof(Guid);
        }

        private static bool HasFromUriAttr(ParameterInfo pInfo)
        {
            return pInfo.GetCustomAttributes(typeof(FromUriAttribute), false).Count() > 0;
        }

        /// <summary>
        /// Allows to create link to resource, relatively app's base url
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        public static Link GetSpecialResource(this UrlHelper urlHelper, string resourcePath, string httpMethod = null)
        {
            return new Link()
            {
                Href = new Uri(GetBaseAddress(urlHelper.Request) + resourcePath),
                Templated = false,
                Method = httpMethod
            };
        }

        private static string GetBaseAddress(HttpRequestMessage requestMessage)
        {
            return requestMessage.RequestUri.Scheme + "://" +
                    requestMessage.RequestUri.Host + ":" + requestMessage.RequestUri.Port +
                    HttpContext.Current.Request.ApplicationPath + "/";
        }
    }
}