using StendenClickerApi.Controllers;
using System.Timers;
using System.Web.Mvc;
using System.Web.Routing;

namespace StendenClickerApi
{
	public class MvcApplication : System.Web.HttpApplication
	{
		public static Timer timer;

		protected void Application_Start()
		{
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			InitTimerAndStartBatchProcessing();
		}

		private void InitTimerAndStartBatchProcessing()
		{
			timer = new Timer();
			timer.Elapsed += Timer_Elapsed;
			timer.Interval = 1000;
			timer.AutoReset = true;
			timer.Start();
		}

		private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			await MultiplayerController.RunTasks();
		}
	}
}
