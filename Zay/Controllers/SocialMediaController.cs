using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zay.Data;
using Zay.Models;

namespace Zay.Controllers
{
    [Authorize]
    public class SocialMediaController : Controller
    {
        private readonly AplicationDbContext _db;
        public SocialMediaController(AplicationDbContext db)
        {
            this._db = db;
        }
        [HttpGet]
        [Route("Admin/SocialMedia")]
        public IActionResult Index()
        {
            ViewBag.SocialMedias = this._db.socialMedias.OrderBy(s => s.DisplayOrder);
            return View();
        }
        [HttpGet]
        [Route("Admin/SocialMedia/Create")]
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        [Route("Admin/SocialMedia/Create")]
        public IActionResult Create(SocialMedia obj)
        {
            if (ModelState.IsValid)
            {
                this._db.socialMedias.Add(obj);
                this._db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }
        [HttpGet]
        [Route("Admin/SocialMedia/Edit/{id}")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = this._db.socialMedias.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        [HttpPost]
        [Route("Admin/SocialMedia/Edit/{id}")]
        public IActionResult Edit(SocialMedia EditModel)
        {
            if (EditModel == null)
            {
                return NotFound();
            }
            this._db.socialMedias.Update(EditModel);
            this._db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("Admin/SocialMedia/Delete/{id}")]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var obj = this._db.socialMedias.Find(id);
                if (obj != null) 
                {
                    this._db.socialMedias.Remove(obj);
                    this._db.SaveChanges();
                    TempData["FlashMessage"] = "Deleted Successfully.";
                    TempData["FlashMessageClass"] = "alert-success";
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
