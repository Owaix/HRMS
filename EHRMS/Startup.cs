using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EHRMS.Startup))]
namespace EHRMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
