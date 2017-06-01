using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SigmalHex.WebDashbord.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.!!!";

            return View();
        }

        /// <summary>
        /// Shows the error page
        /// </summary>
        public async Task<IActionResult> Error(string errorId)
        {
           

            return View();
        }

        public IActionResult Math()
        {
            return View();
        }

        public IActionResult Requirement()
        {
            return View();
        }

        public IActionResult Tasks()
        {
            return View();
        }
    }
}
