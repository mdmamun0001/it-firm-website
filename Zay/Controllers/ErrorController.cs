using Microsoft.AspNetCore.Mvc;
using Zay.Data;

namespace Zay.Controllers
{
    public class ErrorController : Controller
    {
        private readonly AplicationDbContext _db;
        public ErrorController( AplicationDbContext db)
        {
            
            this._db = db;
        }
        [Route("/Error/{StatusCode}")]
        public IActionResult HttpStatusCodeHandaler(int StatusCode)
        {
            switch (StatusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry Expected contet Not Found !"; 
                    break;
            }
            ViewBag.ServiceCategories = this._db.ServiceCategories.Where(s => s.IsActive == true).OrderBy(s => s.DisplayOrder).ToList();
            ViewBag.CompanyInfo = this._db.Abouts.ToList();
            ViewBag.SocialMedias = this._db.socialMedias.Where(s => s.IsActive == true).OrderBy(s => s.DisplayOrder).ToList();
            return View("NotFound");
        }
    }
}
