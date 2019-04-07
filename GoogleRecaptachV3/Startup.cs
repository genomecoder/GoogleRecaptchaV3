using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GoogleRecaptachV3.Startup))]
namespace GoogleRecaptachV3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
