using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.ApplicationContext;
using Web.Services;

namespace Web.Controllers
{
    public class WarsController : Controller
    {     
        WarService _warService;

        public WarsController(WarService warService)
        {
            _warService = warService;
        }

        public IActionResult Index(int? id = null)
        {
            return View();
        }

        public async Task<JsonResult> Last(int? id = null)
        {
            var model = await _warService.FindAsync();
            return Json(model);
        }

        //public bool Exists([FromServices] IActiveWar activeWar)
        //{
        //    return activeWar.Exists;
        //}
    }
}