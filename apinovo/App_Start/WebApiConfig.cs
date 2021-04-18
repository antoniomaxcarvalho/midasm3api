using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Routing;

namespace apinovo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Serviços e configuração da API da Web



            //var cors = new EnableCorsAttribute("http://locahost/apinovomilenio/", "*", "*");
            //    config.EnableCors(cors);

            // Rotas da API da Web
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            //config.Routes.MapHttpRoute(
            //        name: "NovaRota",
            //        routeTemplate: "{controller}/{action}/{id}",
            //        defaults: new { id = RouteParameter.Optional }
            //    );


            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{action}/{id}",
            defaults: new { id = RouteParameter.Optional }
            );

            var enableCorsAttribute = new EnableCorsAttribute("*",
                                               "Origin, Content-Type, Accept",
                                               "GET, PUT, POST, DELETE, OPTIONS");
            config.EnableCors(enableCorsAttribute);

     

            //config.EnableCors();
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            //json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.Indent = true;
        }
    }
}
