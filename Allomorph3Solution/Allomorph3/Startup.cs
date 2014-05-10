using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Allomorph3.Startup))]
namespace Allomorph3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
