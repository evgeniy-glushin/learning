using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Microsoft.AspNetCore.Identity;
using Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Web.ApplicationContext;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        IActiveWar _activeWar;
        private ICurrentUser _player;

        public HomeController(ICurrentUser player, IActiveWar activeWar) 
        {
            _activeWar = activeWar;
            _player = player;
        }

        [Authorize]      
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
