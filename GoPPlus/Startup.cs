using Microsoft.Owin;
using Microsoft.Reporting.WebForms;
using Owin;

[assembly: OwinStartupAttribute(typeof(GoPS.Startup))]
namespace GoPS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
