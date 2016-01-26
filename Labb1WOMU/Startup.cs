using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Labb1WOMU.Startup))]
namespace Labb1WOMU
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
