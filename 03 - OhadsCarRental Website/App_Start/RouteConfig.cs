using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using OhadsCarRental.Constraints;

namespace OhadsCarRental
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Cars
            routes.MapRoute(
                name: "CarsRoute",
                url: "Cars/",
                defaults: new { controller = "FleetCars", action = "Watch" }
            );

            // Order Create
            routes.MapRoute(
                name: "OrderCreateRoute",
                url: "Orders/Create/{licensePlate}/{startDate}/{returnDate}",
                defaults: new { controller = "Orders", action = "Create" },
                constraints: new
                {
                    licensePlate = @"^[a-zA-Z\d]{6,7}$",
                    startDate = new RentalDatesRouteConstraint()
                }
            );

            // Orders
            routes.MapRoute(
                name: "OrdersRoute",
                url: "Orders/{action}",
                defaults: new { controller = "Orders", action = "History" }
            );

            // Search
            routes.MapRoute(
                name: "SearchRoute",
                url: "Search/",
                defaults: new { controller = "FleetCars", action = "Search" }
            );

            // Fleet Cars
            routes.MapRoute(
                name: "FleetCarRoute",
                url: "FleetCars/{action}/{licensePlate}",
                defaults: new { controller = "FleetCars" },
                constraints: new { licensePlate = @"^[a-zA-Z\d]{6,7}$" }
            );

            // Account
            routes.MapRoute(
                name: "AccountRoute",
                url: "Account/",
                defaults: new { controller = "Account", action = "UserProfile" }
            );

            // Contact Us
            routes.MapRoute(
                name: "ContactUsRoute",
                url: "ContactUs/",
                defaults: new { controller = "Contacts", action = "Create" }
            );

            // About
            routes.MapRoute(
                name: "AboutRoute",
                url: "About/",
                defaults: new { controller = "Home", action = "About" }
            );

            // Help
            routes.MapRoute(
                name: "HelpRoute",
                url: "Help/",
                defaults: new { controller = "Home", action = "Help" }
            );

            // Privacy
            routes.MapRoute(
                name: "PrivacyRoute",
                url: "Privacy/",
                defaults: new { controller = "Home", action = "Privacy" }
            );

            // Terms
            routes.MapRoute(
                name: "TermsRoute",
                url: "Terms/",
                defaults: new { controller = "Home", action = "Terms" }
            );

            // Home
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}