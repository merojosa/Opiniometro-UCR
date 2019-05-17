using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Opiniometro_WebApp.Startup))]
namespace Opiniometro_WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
