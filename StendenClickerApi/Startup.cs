using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Newtonsoft.Json;
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

            GlobalHost.Configuration.MaxIncomingWebSocketMessageSize = null;

            var service = (JsonSerializer)GlobalHost.DependencyResolver.GetService(typeof(Newtonsoft.Json.JsonSerializer));
            service.TypeNameHandling = TypeNameHandling.All;
        }
    }
}
