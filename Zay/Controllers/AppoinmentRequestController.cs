using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using X.PagedList;
using Zay.Data;

namespace Zay.Controllers
{
    [Authorize]
    public class AppoinmentRequestController : Controller
    {
        private readonly AplicationDbContext _db;
        public AppoinmentRequestController(AplicationDbContext db)
        {
            this._db = db;
        }
        [Route("Admin/AppoinmentRequest")]
        public IActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10; // Get 25 entities for each requested page.
            ViewBag.AppoinmentRequests = this._db.AppoinmentRequests.OrderByDescending( a => a.CreatedDateTime).ToPagedList(pageNumber, pageSize);
            return View();
        }
        [Route("Admin/AppoinmentRequest/View/{Id}")]
        public IActionResult View(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var AppoinmentRequest = this._db.AppoinmentRequests.Find(Id);
            if (AppoinmentRequest == null)
            {
                return NotFound();
            }
            AppoinmentRequest.IsSeen = true;
            this._db.SaveChanges();
            ViewBag.AppoinmentRequest = AppoinmentRequest;
            return View();
        }
        [Route("Admin/AppoinmentRequest/Delete/{Id}")]
        public IActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var AppoinmentRequest = this._db.AppoinmentRequests.Find(Id);
            if (AppoinmentRequest == null)
            {
                return NotFound();
            }
            this._db.AppoinmentRequests.Remove(AppoinmentRequest);
            this._db.SaveChanges();
            TempData["FlashMessage"] = "Deleted Successfully.";
            TempData["FlashMessageClass"] = "alert-success";
            return RedirectToAction("Index");
        }
        [Route("GetUnSeenAppoinmentRequest")]
        public JsonResult GetUnSeenAppoinmentRequest()
        {
            var allUnSeenAppoinmentRequest = this._db.AppoinmentRequests.Where(ar => ar.IsSeen == false);
            return Json(allUnSeenAppoinmentRequest);
        }
    }
}
