using System;
using System.Web;
using System.Web.Routing;

namespace OhadsCarRental.Constraints
{

    /// <summary>
    /// Represents a constraint for a DateTime in the route.
    /// </summary>
    public class DateTimeRouteConstraint : IRouteConstraint
    {
        #region IRouteConstraint Members

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            DateTime dateTime;
            return DateTime.TryParse(values[parameterName] as string, out dateTime);
        }

        #endregion
    }

}