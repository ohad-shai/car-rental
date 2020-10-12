using OhadsCarRental.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OhadsCarRental
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            if (Context.IsCustomErrorEnabled)
                ShowCustomErrorPage(Server.GetLastError());
        }

        #region Private Methods

        /// <summary>
        /// Shows the custom error page according to the error that occurred!
        /// </summary>
        /// <param name="exception">The thrown exception</param>
        private void ShowCustomErrorPage(Exception exception)
        {
            var httpException = exception as HttpException ?? new HttpException(500, "Internal Server Error", exception);

            Response.Clear();
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("fromAppErrorEvent", true);

            switch (httpException.GetHttpCode())
            {
                case 403:
                    routeData.Values.Add("action", "HttpError403");
                    break;

                case 404:
                    routeData.Values.Add("action", "HttpError404");
                    break;

                case 500:
                    routeData.Values.Add("action", "HttpError500");
                    break;

                default:
                    routeData.Values.Add("action", "Index");
                    routeData.Values.Add("httpStatusCode", httpException.GetHttpCode());
                    break;
            }

            // Log the error... (not necessary in this project)

            Server.ClearError();

            IController controller = new ErrorController();
            controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }

        #endregion

    }
}