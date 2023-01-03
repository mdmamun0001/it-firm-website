using MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;
using Zay.Data;
using Zay.Models;

namespace Zay.Controllers
{
    [Authorize]
    public class CustomerMessageController : Controller
    {
        private readonly AplicationDbContext _db;
        public CustomerMessageController(AplicationDbContext db)
        {
            this._db = db;
        }
        [Route("Admin/CustomerMessage")]
        public IActionResult Index(int? page)
        {
            int pageNumber = page ?? 1; 
            int pageSize = 10; // Get 25 entities for each requested page.
            ViewBag.CustomerMessages = this._db.CustomerMessages.OrderByDescending(cm => cm.CreatedDateTime).ToPagedList(pageNumber, pageSize);
            return View();
        }
        [Route("Admin/CustomerMessage/View/{Id}")]
        public IActionResult View(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var  CustomerMessage = this._db.CustomerMessages.Find(Id);
            if (CustomerMessage == null)
            {
                return NotFound();
            }
            CustomerMessage.IsSeen = true;
            this._db.SaveChanges();
            ViewBag.CustomerMessage = CustomerMessage;
            return View();
        }
        [Route("Admin/CustomerMessage/Delete/{Id}")]
        public IActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var CustomerMessage = this._db.CustomerMessages.Find(Id);
            if (CustomerMessage == null)
            {
                return NotFound();
            }
            this._db.CustomerMessages.Remove(CustomerMessage);
            this._db.SaveChanges();
            TempData["FlashMessage"] = "Deleted Successfully.";
            TempData["FlashMessageClass"] = "alert-success";
            return RedirectToAction("Index");
        }
        [Route("AdminGetAllCustomerMessages")]
        public JsonResult AdminGetAlCustomerMessages()
        {
           var CustomerMessages = this._db.CustomerMessages.Where( cm => cm.IsSeen == false);
            return Json(CustomerMessages);
        }
    }
}
