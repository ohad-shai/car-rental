using System;
using System.Web;
using System.Web.Routing;

namespace OhadsCarRental.Constraints
{

    /// <summary>
    /// Represents a constraint for a checking the rental dates in the route.
    /// Checks the values of "startDate" and "returnDate" in the route, are valid.
    /// </summary>
    public class RentalDatesRouteConstraint : IRouteConstraint
    {
        #region IRouteConstraint Members

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            DateTime startDate;
            DateTime returnDate;

            // Checks if StartDate is DateTime:
            if (DateTime.TryParse(values["startDate"] as string, out startDate))
            {
                // Checks if the Start Date is lower than today's date:
                if (startDate.Date >= DateTime.Today)
                {
                    // Checks if ReturnDate is DateTime:
                    if (DateTime.TryParse(values["returnDate"] as string, out returnDate))
                    {
                        // Checks if ReturnDate is bigger or equal to StartDate:
                        if (returnDate.Date >= startDate.Date)
                        {
                            return true; // ReturnDate is valid.
                        }
                    }
                }
            }

            return false; // If one condition above is false, then the Rental Dates is not valid.
        }

        #endregion
    }

}