using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zay.Data;
using Zay.Models;
using Zay.ViewModel;
using static System.Net.Mime.MediaTypeNames;

namespace Zay.Controllers
{
    [Authorize]
    public class ServiceCategoryController : Controller
    {
        private readonly AplicationDbContext _db;
        private readonly IWebHostEnvironment WebHostEnvironment;
        public ServiceCategoryController(AplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            this._db = db;
            WebHostEnvironment = webHostEnvironment;
        }
        [Route("Admin/ServiceCategory")]
        public IActionResult Index()
        {
            var ServiceCategories = this._db.ServiceCategories.OrderBy(s => s.DisplayOrder);
            ViewData["ServiceCategories"] = ServiceCategories;
            return View();
        }
        [HttpGet]
        [Route("Admin/ServiceCategory/Create")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Route("Admin/ServiceCategory/Create")]

        [ValidateAntiForgeryToken]
        public IActionResult Create(ServiceCategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string? FileName = null;
                if (vm.Image != null )
                {

                    FileName = this.UploadFile(vm.Image);

                }

                if (FileName == null)
                {
                    return View(vm);

                }
            ServiceCategory Obj = new ServiceCategory
                {
                    Name = vm.Name,
                    Description = vm.Description,
                    Images = FileName,
                    DisplayOrder = vm.DisplayOrder,
                    IsActive = vm.IsActive
                };
           
                this._db.ServiceCategories.Add(Obj);
                this._db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vm);
        }
        [HttpGet]
        [Route("Admin/ServiceCategory/Edit/{id}")]
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var model = this._db.ServiceCategories.Find(id);
            if(model == null)
            {
                return NotFound();
            }
            ServiceCategoryEditViewModel obj = new ServiceCategoryEditViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                DisplayOrder= model.DisplayOrder,
                IsActive = model.IsActive,
                ExistingImage = model.Images,

            };
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/ServiceCategory/Edit/{id}")]
        public IActionResult Edit(ServiceCategoryEditViewModel EditModel)
        {
            
                string? FileName = null;
                if (EditModel.Image != null )
                {
                    
                    FileName = this.UploadFile(EditModel.Image);
                    
                    string fileLocation = Path.Combine(WebHostEnvironment.WebRootPath, "img/Service");
                    
                       string filePath = Path.Combine(fileLocation, EditModel.ExistingImage);
                        if(System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }

                     EditModel.ExistingImage = FileName;

                }
                var item = this._db.ServiceCategories.Find(EditModel.Id);
                if(item != null)
                {
                    item.Description = EditModel.Description;
                    item.IsActive = EditModel.IsActive;
                    item.Name = EditModel.Name;
                    item.DisplayOrder = EditModel.DisplayOrder;
                    item.Images = EditModel.ExistingImage;
                    this._db.ServiceCategories.Update(item);
                    this._db.SaveChanges();
                    return RedirectToAction("Index");
                }
            

            
            return View(EditModel);
        }
        [Route("Admin/ServiceCategory/Delete/{id}")]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var SrvcCtrgy = this._db.ServiceCategories.Find(id);
                if (SrvcCtrgy != null)
                {
                    string fileLocation = Path.Combine(WebHostEnvironment.WebRootPath, "img/Service");

                    string filePath = Path.Combine(fileLocation, SrvcCtrgy.Images);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }


                    this._db.ServiceCategories.Remove(SrvcCtrgy);
                    this._db.SaveChanges();
                    TempData["FlashMessage"] = "Deleted Successfully.";
                    TempData["FlashMessageClass"] = "alert-success";
                    return RedirectToAction("Index");
                }
            }

            return NotFound();
        }
        public string? UploadFile(IFormFile image)
        {
            string? fileName = null;
            if(image != null)  
            {
                Random random = new Random();
                if (!Directory.Exists(Path.Combine(WebHostEnvironment.WebRootPath, "img/Service")))
                    Directory.CreateDirectory(Path.Combine(WebHostEnvironment.WebRootPath, "img/Service"));
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "img/Service");
                 fileName = random.Next() + "_" + image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream =  new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }

            return fileName;
        }
    }
}
