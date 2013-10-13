using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SofiaTransport.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            #region GetSchedules   //request Update on User start
            config.Routes.MapHttpRoute(
                                name: "GetSchedules",
                                routeTemplate: "api/Transport/{action}",
                                defaults: new
                                {
                                    controller = "Transport",
                                    action = "GetSchedules"
                                }
                            );
            #endregion


            #region Db/GetNames   //request Update on User start
            config.Routes.MapHttpRoute(
                                name: "GetNames",
                                routeTemplate: "api/Transport/{action}",
                                defaults: new
                                {
                                    controller = "Transport",
                                    action = "GetNames"
                                }
                            );
            #endregion
            #region Db/Update   //request Update on User start
            config.Routes.MapHttpRoute(
                                name: "GetDataType1",
                                routeTemplate: "api/Transport/{action}",
                                defaults: new
                                {
                                    controller = "Transport",
                                    action = "GetDataType1"
                                }
                            );
            #endregion


            #region Db/Update   //request Update on User start
            config.Routes.MapHttpRoute(
                                name: "GetDataType2",
                                routeTemplate: "api/Transport/{action}",
                                defaults: new
                                {
                                    controller = "Transport",
                                    action = "GetDataType2"
                                }
                            );
            #endregion


            #region Db/Update   //request Update on User start
            config.Routes.MapHttpRoute(
                                name: "GetDataType3",
                                routeTemplate: "api/Transport/{action}",
                                defaults: new
                                {
                                    controller = "Transport",
                                    action = "GetDataType3"
                                }
                            );
            #endregion


            #region Db/Update   //request Update on User start
            config.Routes.MapHttpRoute(
                                name: "GetRelations",
                                routeTemplate: "api/Transport/{action}",
                                defaults: new
                                {
                                    controller = "Transport",
                                    action = "GetRelations"
                                }
                            );
            #endregion

            #region Db/Update   //request Update on User start
            config.Routes.MapHttpRoute(
                                name: "InitOrUpdateTodayPublic",
                                routeTemplate: "api/Transport/{action}",
                                defaults: new
                                {
                                    controller = "Transport",
                                    action = "Update"
                                }
                            );
            #endregion










            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
        }
    }
}
