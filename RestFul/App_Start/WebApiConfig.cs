using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RestFul
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            //跨域配置
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "QueryApi",
                routeTemplate: "api/{controller}/{table}/{pageSize}/{page}/{where}/{orderField}",
                defaults: new { table = RouteParameter.Optional, pageSize = RouteParameter.Optional, page = RouteParameter.Optional, where = RouteParameter.Optional , orderField = RouteParameter.Optional }
            );

            /*
            config.Routes.MapHttpRoute(
                name: "DeleteApi",
                routeTemplate: "api/{controller}/{table}/{where}",
                defaults: new { table = RouteParameter.Optional, where = RouteParameter.Optional}
            );
            */
        }
    }
}
