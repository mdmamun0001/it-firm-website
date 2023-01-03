using Microsoft.AspNetCore.Mvc;
using Zay.Data;
using Zay.Models;
using Zay.ViewModel;
using Zay.CustomHelper;
using Microsoft.AspNetCore.Authorization;

namespace Zay.Controllers
{
    [Authorize]
    public class GallaryController : Controller
    {

        private readonly AplicationDbContext _db;
        private readonly IWebHostEnvironment WebHostEnvironment;
        public GallaryController(AplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            this._db = db;
            WebHostEnvironment = webHostEnvironment;
        }
        [Route("Admin/Gallary")]
        public IActionResult Index()
        {
            var gallaries = this._db.gallaries.OrderBy(s => s.DisplayOrder);
            ViewData["gallaries"] = gallaries;
            return View();
        }
        [HttpGet]
        [Route("Admin/Gallary/Create")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Route("Admin/Gallary/Create")]

        [ValidateAntiForgeryToken]
        public IActionResult Create(GallaryCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string? FileName = Helper.UploadFile(vm.Image, "img/Gallary", this.WebHostEnvironment);
                if (FileName == null)
                {
                    return View(vm);
                }
                Gallary Obj = new Gallary
                {
                    Title = vm.Title,
                    Image = FileName,
                    DisplayOrder = vm.DisplayOrder,
                    IsActive = vm.IsActive
                };

                this._db.gallaries.Add(Obj);
                this._db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vm);
        }
        [HttpGet]
        [Route("Admin/Gallary/Edit/{id}")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = this._db.gallaries.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            GallaryEditViewModel obj = new GallaryEditViewModel
            {
                Id = model.Id,
                Title = model.Title,
                DisplayOrder = model.DisplayOrder,
                IsActive = model.IsActive,
                ExistingImage = model.Image,

            };
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/Gallary/Edit/{id}")]
        public IActionResult Edit(GallaryEditViewModel EditModel)
        {

            if (ModelState.IsValid)
            {

                if (EditModel.Image != null)
                {
                    string? NewFileName = Helper.UploadFile(EditModel.Image, "img/Gallary", this.WebHostEnvironment);
                 
                    if (NewFileName != null)
                    {
                        if (EditModel.ExistingImage != null)
                        {
                            Helper.DeleteFile(this.WebHostEnvironment, EditModel.ExistingImage, "img/Gallary");    
                           
                        }
                        EditModel.ExistingImage = NewFileName; 
                    }
                }

                var Element = this._db.gallaries.Find(EditModel.Id);
                if (Element != null)
                {
                    Element.Title = EditModel.Title;
                    Element.IsActive = EditModel.IsActive;
                    Element.DisplayOrder = EditModel.DisplayOrder;
                    Element.Image = EditModel.ExistingImage;
                    this._db.gallaries.Update(Element);
                    this._db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return NotFound();
            }

            return View(EditModel);
        }
        [Route("Admin/Gallary/Delete/{id}")]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var Glry = this._db.gallaries.Find(id);
                if (Glry != null)
                {
                    if (Glry.Image != null)
                    {
                        Helper.DeleteFile(this.WebHostEnvironment, Glry.Image, "img/Gallary");
                    }

                    this._db.gallaries.Remove(Glry);
                    this._db.SaveChanges();
                    TempData["FlashMessage"] = "Deleted Successfully.";
                    TempData["FlashMessageClass"] = "alert-success";
                    return RedirectToAction("Index");
                }
                return NotFound();
            }

            return NotFound();
        }
       /* public string? UploadFile(IFormFile image)
        {
            string? fileName = null;
            if (image != null)
            {
                Guid id = Guid.NewGuid();
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "img/Client");
                fileName = id + "_" + image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }

            return fileName;
        }*/
    }
}
