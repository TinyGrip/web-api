using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using OutdoorSolution.Filters;
using OutdoorSolution.Providers;

namespace OutdoorSolution
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Filters.Add(new ValidateModelAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes(new CentralizedPrefixProvider("api"));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

            // TODO: check ouy what is the best way to ahieve returning always json
            // is it content negotiation or this one?
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
        }
    }
}
