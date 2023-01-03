using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zay.Data;
using Zay.Models;
using Zay.ViewModel;

namespace Zay.Controllers
{
    [Authorize]
    public class ToolController : Controller
    {
        private readonly AplicationDbContext _db;
        private readonly IWebHostEnvironment WebHostEnvironment;
        public ToolController(AplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            this._db = db;
            WebHostEnvironment = webHostEnvironment;
        }
        [Route("Admin/Tool")]
        public IActionResult Index()
        {
            var Tools = this._db.Tools.OrderBy(s => s.DisplayOrder);
            ViewData["Tools"] = Tools;
            return View();
        }
        [HttpGet]
        [Route("Admin/Tool/Create")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Route("Admin/Tool/Create")]

        [ValidateAntiForgeryToken]
        public IActionResult Create(ToolCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string? FileName = this.UploadFile(vm.Image);
                if (FileName == null)
                {
                    return View(vm);
                }
                Tool Obj = new Tool
                {
                    Name = vm.Name,
                    Image = FileName,
                    DisplayOrder = vm.DisplayOrder,
                    IsActive = vm.IsActive
                };

                this._db.Tools.Add(Obj);
                this._db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vm);
        }
        [HttpGet]
        [Route("Admin/Tool/Edit/{id}")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = this._db.Tools.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            ToolEditViewModel obj = new ToolEditViewModel
            {
                ToolId = model.ToolId,
                Name = model.Name,
                DisplayOrder = model.DisplayOrder,
                IsActive = model.IsActive,
                ExistingImage = model.Image,

            };
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/Tool/Edit/{id}")]
        public IActionResult Edit(ToolEditViewModel EditModel)
        {

            if (ModelState.IsValid)
            {

                if (EditModel.Image != null)
                {
                    string NewFileName = this.UploadFile(EditModel.Image);
                    string fileLocation = Path.Combine(WebHostEnvironment.WebRootPath, "img/Tool");
                    if (EditModel.ExistingImage != null)
                    {
                        string ExistingImagePath = Path.Combine(fileLocation, EditModel.ExistingImage.Trim());
                        if (System.IO.File.Exists(ExistingImagePath))
                        {
                            System.IO.File.Delete(ExistingImagePath);
                        }
                    }
                    EditModel.ExistingImage = NewFileName;
                }

                var Element = this._db.Tools.Find(EditModel.ToolId);
                if (Element != null)
                {
                    Element.Name = EditModel.Name;
                    Element.IsActive = EditModel.IsActive;
                    Element.DisplayOrder = EditModel.DisplayOrder;
                    Element.Image = EditModel.ExistingImage;
                    this._db.Tools.Update(Element);
                    this._db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return NotFound();
            }

            return View(EditModel);
        }
        [Route("Admin/Tool/Delete/{id}")]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var ToolObj = this._db.Tools.Find(id);
                if (ToolObj != null)
                {
                    if (ToolObj.Image != null)
                    {
                        string FileLocation = Path.Combine(WebHostEnvironment.WebRootPath, "img/Tool");

                        string ExistingFilePath = Path.Combine(FileLocation, ToolObj.Image.Trim());
                        if (System.IO.File.Exists(ExistingFilePath))
                        {
                            System.IO.File.Delete(ExistingFilePath);
                        }
                    }

                    this._db.Tools.Remove(ToolObj);
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
                if (!Directory.Exists(Path.Combine(WebHostEnvironment.WebRootPath, "img/Tool")))
                    Directory.CreateDirectory(Path.Combine(WebHostEnvironment.WebRootPath, "img/Tool"));
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "img/Tool");
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
