//using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using StendenClickerApi.Database;
using StendenClickerApi.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace StendenClickerApi.Helpers
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
	public class ApiKeySecurityAttribute : AuthorizeAttribute
	{
        private readonly string globalAPIKey;

        public ApiKeySecurityAttribute() : this("1D4AB4D5-2A21-4437-B11D-ED7874A4AB21")
		{
            
		}

        public ApiKeySecurityAttribute(string keyOverride)
		{
            globalAPIKey = keyOverride;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string ip = httpContext.Request.UserHostAddress;
            if(httpContext.Request.Headers["API_KEY"] != null)
			{
                string apiKey = httpContext.Request.Headers["API_KEY"];
				if (apiKey.Equals(globalAPIKey))
				{
                    return true;
				}              
			}
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized, "Your API-KEY does not match");
        }
    }

	public class UserGUIDSecurityAttribute : Microsoft.AspNet.SignalR.AuthorizeAttribute
	{
        StendenClickerDatabase db = new StendenClickerDatabase();

		public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, Microsoft.AspNet.SignalR.IRequest request)
		{
            string userGuid = request.Headers.Get("UserGuid");

			Player p = db.Players.FirstOrDefault(n => n.PlayerGuid == userGuid);

            return p != null;			
		}

		public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
		{
            if(hubIncomingInvokerContext.Hub is MultiplayerHub)
			{
                return true;
			}
			return base.AuthorizeHubMethodInvocation(hubIncomingInvokerContext, appliesToMethod);
		}
	}
}