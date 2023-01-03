using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zay.Data;
using Zay.Models;
using Zay.ViewModel;

namespace Zay.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly AplicationDbContext _db;
        private readonly IWebHostEnvironment WebHostEnvironment;
        public ClientController(AplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            this._db = db;
            WebHostEnvironment = webHostEnvironment;
        }
        [Route("Admin/Client")]
        public IActionResult Index()
        {
            var Clients = this._db.Clients.OrderBy(s => s.DisplayOrder);
            ViewData["Clients"] = Clients;
            return View();
        }
        [HttpGet]
        [Route("Admin/Client/Create")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Route("Admin/Client/Create")]

        [ValidateAntiForgeryToken]
        public IActionResult Create(ClientCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string? FileName = this.UploadFile(vm.Image);
                Client Obj = new Client
                {
                    Name = vm.Name,
                    Description = vm.Description,
                    Image = FileName,
                    DisplayOrder = vm.DisplayOrder,
                    IsActive = vm.IsActive
                };

                this._db.Clients.Add(Obj);
                this._db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vm);
        }
        [HttpGet]
        [Route("Admin/Client/Edit/{id}")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = this._db.Clients.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            ClientEditViewModel obj = new ClientEditViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                DisplayOrder = model.DisplayOrder,
                IsActive = model.IsActive,
                ExistingImage = model.Image,

            };
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/Client/Edit/{id}")]
        public IActionResult Edit(ClientEditViewModel EditModel)
        {

            if (ModelState.IsValid)
            {

                if (EditModel.Image != null)
                {
                    string NewFileName = this.UploadFile(EditModel.Image);
                    string fileLocation = Path.Combine(WebHostEnvironment.WebRootPath, "img/Client");
                    if(EditModel.ExistingImage != null)
                    {
                        string ExistingImagePath = Path.Combine(fileLocation, EditModel.ExistingImage.Trim());
                        if (System.IO.File.Exists(ExistingImagePath))
                        {
                            System.IO.File.Delete(ExistingImagePath);
                        }
                    }
                    EditModel.ExistingImage = NewFileName;
                }

                var Element = this._db.Clients.Find(EditModel.Id);
                if (Element != null)
                {
                    Element.Name = EditModel.Name;
                    Element.Description = EditModel.Description;
                    Element.IsActive = EditModel.IsActive;
                    Element.DisplayOrder = EditModel.DisplayOrder;
                    Element.Image = EditModel.ExistingImage;
                    this._db.Clients.Update(Element);
                    this._db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return NotFound();
            }

            return View(EditModel);
        }
        [Route("Admin/Client/Delete/{id}")]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var Clnt = this._db.Clients.Find(id);
                if (Clnt != null)
                {
                    if (Clnt.Image != null)
                    {
                        string FileLocation = Path.Combine(WebHostEnvironment.WebRootPath, "img/Client");

                        string ExistingFilePath = Path.Combine(FileLocation, Clnt.Image.Trim());
                        if (System.IO.File.Exists(ExistingFilePath))
                        {
                            System.IO.File.Delete(ExistingFilePath);
                        }
                    }

                    this._db.Clients.Remove(Clnt);
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
                if (!Directory.Exists(Path.Combine(WebHostEnvironment.WebRootPath, "img/Client")))
                    Directory.CreateDirectory(Path.Combine(WebHostEnvironment.WebRootPath, "img/Client"));
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "img/Client");
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
