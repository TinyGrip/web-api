using OutdoorSolution.Dto;
using OutdoorSolution.Dto.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Http;
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
            // Mody expression has to be method in order to read parameters
            MethodCallExpression bodyExpr = expr.Body as MethodCallExpression;
            if (bodyExpr == null)
                throw new Exception("Wrong expression!");

            // dictionary of result url parameters for controller's action
            var urlParamsDictionary = new Dictionary<string, object>();
            
            var methodParams = bodyExpr.Method.GetParameters();
            foreach (var p in methodParams)
            {
                // get parameter's value
                var argDelegate = Expression.Lambda(bodyExpr.Arguments[p.Position]).Compile();
                var currArgument = argDelegate.DynamicInvoke();

                // save name value pair depending on parameter type
                if (IsSystemType(p.ParameterType)) // system type value should be saved as is
                {
                    urlParamsDictionary.Add(p.Name, currArgument);
                }
                else if (HasFromUriAttr(p)) // in case of user type - property name-values should be extracted 
                {
                    ReadObjectProperties(urlParamsDictionary, p.ParameterType, currArgument);
                }
            }

            // add controller name in case we create link to other than current controller
            urlParamsDictionary.Add("controller", typeof(T).Name.Replace("Controller", ""));

            // create hypermedia link obj
            var link = new Link();
            link.Href = new Uri(urlHelper.Link("DefaultApi", urlParamsDictionary));
            link.Templated = false;

            return link;
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

                dictionary.Add(objProp.Name, objPropValue);
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
        /// Returns link to resource, that is not exposed through web api controller
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        public static Link GetSpecialResource(this UrlHelper urlHelper, string resourcePath, HttpMethods? httpMethod = null)
        {
            var requestUri = urlHelper.Request.RequestUri;
            return new Link()
            {
                Href = new Uri(requestUri.Scheme + "://" +
                    requestUri.Host + ":" + requestUri.Port +
                    HttpContext.Current.Request.ApplicationPath + "/" +
                    resourcePath),
                Templated = true,
                Method = httpMethod
            };
        }
    }
}