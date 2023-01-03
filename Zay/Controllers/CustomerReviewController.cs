using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zay.Data;
using Zay.Models;
using Zay.ViewModel;

namespace Zay.Controllers
{
    [Authorize]
    public class CustomerReviewController : Controller
    {
        private readonly AplicationDbContext _db;
        private readonly IWebHostEnvironment WebHostEnvironment;
        public CustomerReviewController(AplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            this._db = db;
            WebHostEnvironment = webHostEnvironment;
        }
        [Route("Admin/CustomerReview")]
        public IActionResult Index()
        {
            var CustomerReviews = this._db.CustomerReviews.OrderBy(s => s.DisplayOrder);
            ViewData["CustomerReviews"] = CustomerReviews;
            return View();
        }
        [HttpGet]
        [Route("Admin/CustomerReview/Create")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Route("Admin/CustomerReview/Create")]

        [ValidateAntiForgeryToken]
        public IActionResult Create(CustomerReviewCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string? FileName = this.UploadFile(vm.Image);
                CustomerReview Obj = new CustomerReview
                {
                    Name = vm.Name,
                    Description = vm.Description,
                    Message = vm.Message,
                    Image = FileName,
                    DisplayOrder = vm.DisplayOrder,
                    IsActive = vm.IsActive
                };

                this._db.CustomerReviews.Add(Obj);
                this._db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vm);
        }
        [HttpGet]
        [Route("Admin/CustomerReview/Edit/{id}")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = this._db.CustomerReviews.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            CustomerReviewEditViewModel obj = new CustomerReviewEditViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Message=model.Message,
                DisplayOrder = model.DisplayOrder,
                IsActive = model.IsActive,
                ExistingImage = model.Image,

            };
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/CustomerReview/Edit/{id}")]
        public IActionResult Edit(CustomerReviewEditViewModel EditModel)
        {

            if (ModelState.IsValid)
            {

                if (EditModel.Image != null)
                {
                    string NewFileName = this.UploadFile(EditModel.Image);
                    string fileLocation = Path.Combine(WebHostEnvironment.WebRootPath, "img/Reviewer");
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

                var Element = this._db.CustomerReviews.Find(EditModel.Id);
                if (Element != null)
                {
                    Element.Name = EditModel.Name;
                    Element.Description = EditModel.Description;
                    Element.Message = EditModel.Message;
                    Element.IsActive = EditModel.IsActive;
                    Element.DisplayOrder = EditModel.DisplayOrder;
                    Element.Image = EditModel.ExistingImage;
                    this._db.CustomerReviews.Update(Element);
                    this._db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return NotFound();
            }

            return View(EditModel);
        }
        [Route("Admin/CustomerReview/Delete/{id}")]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var CstmrRvw = this._db.CustomerReviews.Find(id);
                if (CstmrRvw != null)
                {
                    if (CstmrRvw.Image != null)
                    {
                        string FileLocation = Path.Combine(WebHostEnvironment.WebRootPath, "img/Reviewer");

                        string ExistingFilePath = Path.Combine(FileLocation, CstmrRvw.Image.Trim());
                        if (System.IO.File.Exists(ExistingFilePath))
                        {
                            System.IO.File.Delete(ExistingFilePath);
                        }
                    }

                    this._db.CustomerReviews.Remove(CstmrRvw);
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
                if (!Directory.Exists(Path.Combine(WebHostEnvironment.WebRootPath, "img/Reviewer")))
                    Directory.CreateDirectory(Path.Combine(WebHostEnvironment.WebRootPath, "img/Reviewer"));
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "img/Reviewer");
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
