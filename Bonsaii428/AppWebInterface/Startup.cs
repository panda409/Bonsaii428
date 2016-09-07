using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AppWebInterface.Startup))]
namespace AppWebInterface
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
