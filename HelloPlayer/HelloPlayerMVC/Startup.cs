using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HelloPlayerMVC.Startup))]
namespace HelloPlayerMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
