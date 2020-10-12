using System.Web;
using System.Web.Optimization;

namespace OhadsCarRental
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            #region Themes Styles (Styles for particular pages)

            bundles.Add(new StyleBundle("~/Content/themes/Orders").Include("~/Content/themes/Orders.css"));

            bundles.Add(new StyleBundle("~/Content/themes/FleetCars").Include("~/Content/themes/FleetCars.css"));

            bundles.Add(new StyleBundle("~/Content/themes/Account").Include("~/Content/themes/Account.css"));

            bundles.Add(new StyleBundle("~/Content/themes/Contacts").Include("~/Content/themes/Contacts.css"));

            bundles.Add(new StyleBundle("~/Content/themes/FunnyPages").Include("~/Content/themes/FunnyPages.css"));

            bundles.Add(new StyleBundle("~/Content/themes/About").Include("~/Content/themes/About.css"));

            bundles.Add(new StyleBundle("~/Content/themes/Home").Include("~/Content/themes/Home.css"));

            bundles.Add(new StyleBundle("~/Content/themes/Error").Include("~/Content/themes/Error.css"));

            #endregion

            #region Page Scripts

            // Order Create page script:
            bundles.Add(new ScriptBundle("~/bundles/OrderCreate").Include(
                        "~/Scripts/OrderCreate.js"));

            // Search page script:
            bundles.Add(new ScriptBundle("~/bundles/CarsSearcher").Include(
                        "~/Scripts/CarsSearcher.js"));

            // HomePage script:
            bundles.Add(new ScriptBundle("~/bundles/Home").Include(
                        "~/Scripts/Home.js"));

            #endregion

            #region Validation Scripts

            bundles.Add(new ScriptBundle("~/bundles/OrderValidations").Include(
                "~/Scripts/CustomValidations/DateGreaterThanToday.js",
                "~/Scripts/CustomValidations/DateGreaterThanProperty.js"));

            bundles.Add(new ScriptBundle("~/bundles/RegisterValidations").Include(
                "~/Scripts/CustomValidations/BirthDateValidation.js",
                "~/Scripts/CustomValidations/IdentityNumberValidation.js"));

            #endregion

            #region General

            // DropDownLists scripts:
            bundles.Add(new ScriptBundle("~/bundles/DropDownLists").Include(
                        "~/Scripts/DropDownLists.js"));

            // Site scripts:
            bundles.Add(new ScriptBundle("~/bundles/OhadsCarRental").Include(
                        "~/Scripts/OhadsCarRental.js"));

            // Site styles:
            bundles.Add(new StyleBundle("~/Content/site").Include("~/Content/Site.css"));

            #endregion

            #region Libraries

            bundles.Add(new ScriptBundle("~/bundles/webflow").Include(
                "~/Scripts/webflow.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            #endregion

        }
    }
}