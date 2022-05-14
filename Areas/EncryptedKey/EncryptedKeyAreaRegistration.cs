using System.Web.Mvc;

namespace ExwhyzeeEDI.Web.Areas.EncryptedKey
{
    public class EncryptedKeyAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "EncryptedKey";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "EncryptedKey_default",
                "EncryptedKey/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}