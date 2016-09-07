using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bonsaii.Startup))]
namespace Bonsaii
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
