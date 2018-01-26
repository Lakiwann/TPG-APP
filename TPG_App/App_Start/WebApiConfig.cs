using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.Http.OData.Extensions;
using Audit.EntityFramework.Providers;
using Audit.Core;

namespace TPG_App
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.EnableCors();
            config.AddODataQueryFilter();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            Audit.Core.Configuration.DataProvider = new EntityFrameworkDataProvider();

            Audit.Core.Configuration.Setup()
                .UseEntityFramework(x => x
                .AuditTypeNameMapper(typeName => typeName + "History")
                .AuditEntityAction((evt, entry, auditEntity) =>
                {
                    var a = (dynamic)auditEntity;
                    a.Date = DateTime.UtcNow;
                    a.UserName = evt.Environment.UserName;
                    a.Action = entry.Action; // Insert, Update, Delete
                }));
        }
    }
}
