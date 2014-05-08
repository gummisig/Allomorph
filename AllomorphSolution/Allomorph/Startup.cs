using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Allomorph.Startup))]
namespace Allomorph
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
