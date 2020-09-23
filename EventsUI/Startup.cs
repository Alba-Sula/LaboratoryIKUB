using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EventsUI.Startup))]
namespace EventsUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
