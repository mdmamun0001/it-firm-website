using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zay.Data;
using Zay.Models;
using Zay.ViewModel;

namespace Zay.Controllers
{
    [Authorize]
    public class MailAddressController : Controller
    {
        private readonly AplicationDbContext _db;
        public MailAddressController(AplicationDbContext db)
        {
            this._db = db;
        }
        [HttpGet]
        [Route("Admin/MailAddress")]
        public IActionResult Index()
        {
            ViewBag.MailAddress = this._db.MailAddresses;
            return View();
        }
        [HttpGet]
        [Route("Admin/MailAddress/Create")]
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        [Route("Admin/MailAddress/Create")]
        public IActionResult Create(MailAddressCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                MailAddress newMailAddress = new MailAddress
                {
                    Email = vm.Email,
                    IsActive = vm.IsActive
                };
                this._db.MailAddresses.Add(newMailAddress);
                this._db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }
        [HttpGet]
        [Route("Admin/MailAddress/Edit/{id}")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = this._db.MailAddresses.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            MailAddressEditViewModel Vm = new MailAddressEditViewModel
            {   Id = model.Id,
                Email = model.Email,
                IsActive = model.IsActive
            };
            return View(Vm);
        }
        [HttpPost]
        [Route("Admin/MailAddress/Edit/{id}")]
        public IActionResult Edit(MailAddressEditViewModel EditModel)
        {
            if (ModelState.IsValid)
            {

                var Model = this._db.MailAddresses.Find(EditModel.Id);
                if (Model == null)
                {
                    return NotFound();
                }
                Model.Email = EditModel.Email;
                Model.IsActive = EditModel.IsActive;
                this._db.MailAddresses.Update(Model);
                this._db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(EditModel);
        }
        [HttpGet]
        [Route("Admin/MailAddress/Delete/{id}")]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var obj = this._db.MailAddresses.Find(id);
                if (obj != null)
                {
                    this._db.MailAddresses.Remove(obj);
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
