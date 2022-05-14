using System.Web.Mvc;

namespace ExwhyzeeEDI.Web.Areas.EDI
{
    public class EDIAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "EDI";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "EDI_default",
                "EDI/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}