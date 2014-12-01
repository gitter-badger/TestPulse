using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestPulse.TestApplication.WebUI.Startup))]
namespace TestPulse.TestApplication.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
