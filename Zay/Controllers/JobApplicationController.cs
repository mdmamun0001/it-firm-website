using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Linq;
using X.PagedList;
using Zay.Data;
using Zay.Models;

namespace Zay.Controllers
{
    [Authorize]
    public class JobApplicationController : Controller
    {
        private readonly AplicationDbContext _db;
        public JobApplicationController(AplicationDbContext db)
        {
            this._db = db;
        }
        [Route("Admin/JobApplication")]
        public IActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10; // Get 25 entities for each requested page.
            ViewBag.JobApplications = this._db.JobApplications.OrderByDescending(ja => ja.CreatedDateTime).ToPagedList(pageNumber, pageSize);
            return View();
        }
        [Route("Admin/JobApplication/View/{Id}")]
        public IActionResult View(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var JobApplication = this._db.JobApplications.Find(Id);
            if (JobApplication == null)
            {
                return NotFound();
            }
            JobApplication.IsSeen = true;
            this._db.SaveChanges();
            ViewBag.JobApplication = JobApplication;
            return View();
        }
        [Route("Admin/JobApplication/Delete/{Id}")]
        public IActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var JobApplication = this._db.JobApplications.Find(Id);
            if (JobApplication == null)
            {
                return NotFound();
            }
            this._db.JobApplications.Remove(JobApplication);
            this._db.SaveChanges();
            TempData["FlashMessage"] = "Deleted Successfully.";
            TempData["FlashMessageClass"] = "alert-success";
            return RedirectToAction("Index");
        }
        [Route("GetUnSeenJobApplication")]
        public JsonResult GetUnSeenJobApplication()
        {
            var CustomerMessages = this._db.JobApplications.Where(cm => cm.IsSeen == false);
            return Json(CustomerMessages);
        }

    }
}
