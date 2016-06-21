using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Signar.Startup))]
namespace Signar
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
