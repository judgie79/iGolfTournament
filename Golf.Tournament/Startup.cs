using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Golf.Tournament.Startup))]
namespace Golf.Tournament
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
