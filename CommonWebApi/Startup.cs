using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CommonWebApi.App_Start.Startup))]

namespace CommonWebApi.App_Start
{
    public partial class Startup
    {
    }
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}