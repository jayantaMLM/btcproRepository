using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BtcProApp.Startup))]
namespace BtcProApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
