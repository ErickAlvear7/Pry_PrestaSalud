using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Pry_PrestaSalud.Startup))]
namespace Pry_PrestaSalud
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
