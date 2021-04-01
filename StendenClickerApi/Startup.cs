using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(StendenClickerApi.Startup))]

namespace StendenClickerApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
			var hubConfiguration = new HubConfiguration
			{
				EnableDetailedErrors = true
			};
			app.MapSignalR(hubConfiguration);
        }
    }
}
