using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zay.Data;
using Zay.Models;
using Zay.ViewModel;

namespace Zay.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {
        private readonly AplicationDbContext _db;
        private readonly IWebHostEnvironment WebHostEnvironment;
        public ServiceController(AplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            this._db = db;
            WebHostEnvironment = webHostEnvironment;
        }
        [Route("Admin/Service")]
        public IActionResult Index()
        {
            ViewBag.Services = this._db.Services.Include("ServiceCategories").OrderBy(s => s.DisplayOrder);
            return View();
        }
        [HttpGet]
        [Route("Admin/Service/Create")]
        public IActionResult Create()
        {
            List<ServiceCategory> ServiceCategories = new List<ServiceCategory>();
            ServiceCategories = this._db.ServiceCategories.ToList();
            ViewData["ServiceCategories"] = ServiceCategories;
            return View();
        }
        [HttpPost]
        [Route("Admin/Service/Create")]

        [ValidateAntiForgeryToken]
        public IActionResult Create(ServiceCreateViewModel vm)
        {

            if (ModelState.IsValid)
            {
                string? FileName = this.UploadFile(vm.Image);
                Service Obj = new Service
                {
                    Name = vm.Name,
                    ShortDescription = vm.ShortDescription,
                    Description = vm.Description,
                    Image = FileName,
                    DisplayOrder = vm.DisplayOrder,
                    IsActive = vm.IsActive,
                    ServiceCategories = new List<ServiceCategory>()
                };
                if (vm.ServiceCategoryIds != null)
                {
                    foreach (var ServiceCategory_id in vm.ServiceCategoryIds)
                    {
                        var ServiceCategory = this._db.ServiceCategories.Find(ServiceCategory_id);
                        if (ServiceCategory != null)
                        {
                            Obj.ServiceCategories.Add(ServiceCategory);
                        }
                    }
                }
                this._db.Services.Add(Obj);
                this._db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vm);
        }
         [HttpGet]
         [Route("Admin/Service/Edit/{id}")]
         public IActionResult Edit(int? id)
         {
             if (id == null)
             {
                 return NotFound();
             }
             var model = this._db.Services.Include("ServiceCategories").Where(p => p.ServiceId == id).First();
             if (model == null)
             {
                 return NotFound();
             }
             List<int> ExistingServiceCategoriesIds = new List<int>();
             if (model.ServiceCategories != null)
             {
                 foreach (var item in model.ServiceCategories)
                 {
                    ExistingServiceCategoriesIds.Add(item.Id);
                 }
             }
            ServiceEditViewModel obj = new ServiceEditViewModel
             {
                 ServiceId = model.ServiceId,
                 Name = model.Name,
                 ShortDescription = model.ShortDescription,
                 Description = model.Description,
                 DisplayOrder = model.DisplayOrder,
                 IsActive = model.IsActive,
                 ExistingServiceCategoriesIds = ExistingServiceCategoriesIds,
                 NewServiceCategoriesIds = ExistingServiceCategoriesIds,
                 ExistingImage = model.Image,

             };
             List<ServiceCategory> ServiceCategories = new List<ServiceCategory>();
            ServiceCategories = this._db.ServiceCategories.ToList();
             ViewData["ServiceCategories"] = ServiceCategories;
             return View(obj);
         }
         [HttpPost]
         [ValidateAntiForgeryToken]
         [Route("Admin/Service/Edit/{id}")]
         public IActionResult Edit(ServiceEditViewModel EditModel)
         {


             if (ModelState.IsValid)
             {

                 if (EditModel.Image != null)
                 {
                     string NewFileName = this.UploadFile(EditModel.Image);
                     string fileLocation = Path.Combine(WebHostEnvironment.WebRootPath, "img/Service");
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

                 var Element = this._db.Services.Include("ServiceCategories").Where(s => s.ServiceId == EditModel.ServiceId).First();
                 if (Element != null)
                 {

                     Element.Name = EditModel.Name;
                     Element.ShortDescription = EditModel.ShortDescription;
                     Element.Description = EditModel.Description;
                     Element.IsActive = EditModel.IsActive;
                     Element.DisplayOrder = EditModel.DisplayOrder;
                     Element.Image = EditModel.ExistingImage;
                     //  IEnumerable<int> ShouldRemoveIds = new List<int>();
                     //  IEnumerable<int> ShouldAddIds = new List<int>();
                     if (EditModel.ExistingServiceCategoriesIds != null && EditModel.NewServiceCategoriesIds != null)
                     {
                         this.RemoveToolsToProject(EditModel.ExistingServiceCategoriesIds, EditModel.NewServiceCategoriesIds, Element);
                         this.AddToolsToProject(EditModel.ExistingServiceCategoriesIds, EditModel.NewServiceCategoriesIds, Element);
                     }
                     else if (EditModel.ExistingServiceCategoriesIds == null && EditModel.NewServiceCategoriesIds != null)
                     {
                         this.AddToolsToProject(new List<int>(), EditModel.NewServiceCategoriesIds, Element);
                     }
                     else if (EditModel.ExistingServiceCategoriesIds != null && EditModel.ExistingServiceCategoriesIds.Count() > 0 && EditModel.NewServiceCategoriesIds == null)
                     {
                         foreach (var item in EditModel.ExistingServiceCategoriesIds)
                         {
                             var RmvItm = this._db.ServiceCategories.Find(item);
                              Element.ServiceCategories.Remove(RmvItm);

                         }
                     }

                     this._db.Services.Update(Element);
                     this._db.SaveChanges();
                     return RedirectToAction("Index");
                 }
                 return NotFound();
             }
            return View(EditModel);
         }
        [Route("Admin/Service/Delete/{id}")]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var Srvcs = this._db.Services.Find(id);
                if (Srvcs != null)
                {
                    if (Srvcs.Image != null)
                    {
                        string FileLocation = Path.Combine(WebHostEnvironment.WebRootPath, "img/Service");

                        string ExistingFilePath = Path.Combine(FileLocation, Srvcs.Image.Trim());
                        if (System.IO.File.Exists(ExistingFilePath))
                        {
                            System.IO.File.Delete(ExistingFilePath);
                        }
                    }

                    this._db.Services.Remove(Srvcs);
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
                if (!Directory.Exists(Path.Combine(WebHostEnvironment.WebRootPath, "img/Service")))
                    Directory.CreateDirectory(Path.Combine(WebHostEnvironment.WebRootPath, "img/Service"));
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "img/Service");
                fileName = id + "_" + image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }

            return fileName;
        }
        public void RemoveToolsToProject(List<int> ExistingIds, List<int> NewIds, Service Obj)
        {
            IEnumerable<int> ShouldRemoveIds = ExistingIds.Except(NewIds);
            if (ShouldRemoveIds != null && ShouldRemoveIds.Count() > 0)
            {
                foreach (var item in ShouldRemoveIds)
                {
                    var RmvItm = this._db.ServiceCategories.Find(item);
                    Obj.ServiceCategories.Remove(RmvItm);

                }
            }
        }
        public void AddToolsToProject(List<int> ExistingIds, List<int> NewIds, Service Obj)
        {
            IEnumerable<int> ShouldAddIds = NewIds.Except(ExistingIds);
            if (ShouldAddIds != null && ShouldAddIds.Count() > 0)
            {
                foreach (var item in ShouldAddIds)
                {
                    var AddItm = this._db.ServiceCategories.Find(item);
                    Obj.ServiceCategories.Add(AddItm);

                }
            }
        }
    }
}
