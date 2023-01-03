using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zay.Data;
using Zay.Models;
using Zay.ViewModel;

namespace Zay.Controllers
{
    [Authorize]
    public class EmployeController : Controller
    {
        private readonly AplicationDbContext _db;
        private readonly IWebHostEnvironment WebHostEnvironment;
        public EmployeController(AplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            this._db = db;
            WebHostEnvironment = webHostEnvironment;
        }
        [Route("Admin/Employe")]
        public IActionResult Index()
        {
            var Employes = this._db.Employes.OrderBy(s => s.DisplayOrder);
            ViewData["Employes"] = Employes;
            return View();
        }
        [HttpGet]
        [Route("Admin/Employe/Create")]
        public IActionResult Create()
        {
            return View();
        }
       [HttpPost]
        [Route("Admin/Employe/Create")]

        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string? FileName = this.UploadFile(vm.Image);
                if (FileName == null)
                {
                    return View(vm);
                }
                Employe Obj = new Employe
                {
                    Name = vm.Name,
                    JobTitle = vm.JobTitle,
                    Responsibility = vm.Responsibility,
                    Image = FileName,
                    DisplayOrder = vm.DisplayOrder,
                    IsActive = vm.IsActive
                };

                this._db.Employes.Add(Obj);
                this._db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vm);
        }
        [HttpGet]
        [Route("Admin/Employe/Edit/{id}")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = this._db.Employes.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            EmployeEditViewModel obj = new EmployeEditViewModel
            {
                Id = model.Id,
                Name = model.Name,
                JobTitle = model.JobTitle,
                Responsibility = model.Responsibility,
                IsActive = model.IsActive,
                DisplayOrder = model.DisplayOrder,
                ExistingImage = model.Image,

            };
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/Employe/Edit/{id}")]
        public IActionResult Edit(EmployeEditViewModel EditModel)
        {

            if (ModelState.IsValid)
            {

                if (EditModel.Image != null)
                {
                    string NewFileName = this.UploadFile(EditModel.Image);
                    string fileLocation = Path.Combine(WebHostEnvironment.WebRootPath, "img/Employe");
                    string ExistingImagePath = Path.Combine(fileLocation, EditModel.ExistingImage.Trim());
                    if (System.IO.File.Exists(ExistingImagePath))
                    {
                        System.IO.File.Delete(ExistingImagePath);
                    }
                    EditModel.ExistingImage = NewFileName;
                }

                var Element = this._db.Employes.Find(EditModel.Id);
                if (Element != null)
                {
                    Element.Name = EditModel.Name;
                    Element.JobTitle = EditModel.JobTitle;
                    Element.Responsibility = EditModel.Responsibility;
                    Element.IsActive = EditModel.IsActive;
                    Element.DisplayOrder = EditModel.DisplayOrder;
                    Element.Image = EditModel.ExistingImage;
                    this._db.Employes.Update(Element);
                    this._db.SaveChanges();
                    return RedirectToAction("Index");
                };
                return NotFound();
            }

            return View(EditModel);
        }
        [Route("Admin/Employe/Delete/{id}")]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var Emply = this._db.Employes.Find(id);
                if (Emply != null)
                {
                    string FileLocation = Path.Combine(WebHostEnvironment.WebRootPath, "img/Employe");

                    string ExistingFilePath = Path.Combine(FileLocation, Emply.Image.Trim());
                    if (System.IO.File.Exists(ExistingFilePath))
                    {
                        System.IO.File.Delete(ExistingFilePath);
                    }

                    this._db.Employes.Remove(Emply);
                    this._db.SaveChanges();
                    TempData["FlashMessage"] = "Deleted Successfully.";
                    TempData["FlashMessageClass"] = "alert-success";
                    return RedirectToAction("Index");
                }
                return NotFound();
            }

            return NotFound();
        }
        public string UploadFile(IFormFile image)
        {
            string? fileName = null;
            if (image != null)
            {
                Guid id = Guid.NewGuid();
                if (!Directory.Exists(Path.Combine(WebHostEnvironment.WebRootPath, "img/Employe")))
                    Directory.CreateDirectory(Path.Combine(WebHostEnvironment.WebRootPath, "img/Employe"));
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "img/Employe");
                fileName = id + "_" + image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }

            return fileName;
        }
    }
}
