using Microsoft.AspNetCore.Mvc;

namespace IFFCO.NERRS.Web.Areas.M1.Controllers
{
    public class NERSC01Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
