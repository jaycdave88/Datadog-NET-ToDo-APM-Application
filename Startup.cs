using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Datadog_MVC_ToDo.Startup))]
namespace Datadog_MVC_ToDo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
