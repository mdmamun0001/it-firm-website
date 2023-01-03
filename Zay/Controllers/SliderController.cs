using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zay.Data;
using Zay.Models;
using Zay.ViewModel;
using static System.Net.Mime.MediaTypeNames;

namespace Zay.Controllers
{
    [Authorize]
    public class SliderController : Controller
    {

        private readonly AplicationDbContext _db;
        private readonly IWebHostEnvironment WebHostEnvironment;
        public SliderController(AplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            this._db = db;
            WebHostEnvironment = webHostEnvironment;
        }
        [Route("Admin/Slider")]
        public IActionResult Index()
        {
            var Sliders = this._db.Sliders.OrderBy(s => s.DisplayOrder);
            ViewData["Sliders"] = Sliders;
            return View();
        }
        [HttpGet]
        [Route("Admin/Slider/Create")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Route("Admin/Slider/Create")]

        [ValidateAntiForgeryToken]
        public IActionResult Create(SliderCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string? FileName = this.UploadFile(vm.Image);
                if (FileName == null)
                {
                    return View(vm);
                }
                Slider Obj = new Slider
                {
                    Title = vm.Title,
                    Content = vm.Content,
                    Image = FileName,
                    DisplayOrder = vm.DisplayOrder,
                    IsActive = vm.IsActive
                };

                this._db.Sliders.Add(Obj);
                this._db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vm);
        }
       [HttpGet]
        [Route("Admin/Slider/Edit/{id}")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = this._db.Sliders.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            SliderEditViewModel obj = new SliderEditViewModel
            {
                Id = model.Id,
                Title = model.Title,
                Content = model.Content,
                DisplayOrder = model.DisplayOrder,
                IsActive = model.IsActive,
                ExistingImage = model.Image,

            };
            return View(obj);
        }
       [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/Slider/Edit/{id}")]
        public IActionResult Edit(SliderEditViewModel EditModel)
        {

            if (ModelState.IsValid)
            {
               
                if (EditModel.Image != null)
                {
                    string NewFileName = this.UploadFile(EditModel.Image);
                    string fileLocation = Path.Combine(WebHostEnvironment.WebRootPath, "img/Slider");
                    string ExistingImagePath = Path.Combine(fileLocation, EditModel.ExistingImage.Trim());
                    if (System.IO.File.Exists(ExistingImagePath))
                    {
                        System.IO.File.Delete(ExistingImagePath);
                    }
                    EditModel.ExistingImage = NewFileName;
                }

                var Element = this._db.Sliders.Find(EditModel.Id);
                if (Element != null)
                {
                    Element.Title = EditModel.Title;
                    Element.Content = EditModel.Content;
                    Element.IsActive = EditModel.IsActive;
                    Element.DisplayOrder = EditModel.DisplayOrder;
                    Element.Image = EditModel.ExistingImage;
                    this._db.Sliders.Update(Element);
                    this._db.SaveChanges();
                    return RedirectToAction("Index");
                };
                return NotFound();
            }

            return View(EditModel);
        }
       [Route("Admin/Slider/Delete/{id}")]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var Sldr = this._db.Sliders.Find(id);
                if (Sldr != null)
                {
                    string FileLocation = Path.Combine(WebHostEnvironment.WebRootPath, "img/Slider");
                 
                        string ExistingFilePath = Path.Combine(FileLocation, Sldr.Image.Trim());
                        if (System.IO.File.Exists(ExistingFilePath))
                        {
                            System.IO.File.Delete(ExistingFilePath);
                        }

                    this._db.Sliders.Remove(Sldr);
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
                if (!Directory.Exists(Path.Combine(WebHostEnvironment.WebRootPath, "img/slider")))
                    Directory.CreateDirectory(Path.Combine(WebHostEnvironment.WebRootPath, "img/slider"));
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "img/slider");
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
