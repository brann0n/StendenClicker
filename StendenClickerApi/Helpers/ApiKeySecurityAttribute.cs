using System;
using System.Collections.Generic;
using System.Linq;
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
}