using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExwhyzeeEDI.Web.Startup))]
namespace ExwhyzeeEDI.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
