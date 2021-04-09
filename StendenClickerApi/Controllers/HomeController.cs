using StendenClickerApi.Helpers;
using System.Web.Mvc;

namespace StendenClickerApi.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View(SessionExtensions.Get());
		}
	}
}