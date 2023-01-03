using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Reflection;
using Zay.Data;
using Zay.Models;
using Zay.ViewModel;

namespace Zay.Controllers
{
    [Authorize]
    public class AboutController : Controller
    {
        private readonly AplicationDbContext _db;
        private readonly IWebHostEnvironment WebHostEnvironment;
        public AboutController(AplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            this._db = db;
            WebHostEnvironment = webHostEnvironment;
        }
        [Route("Admin/About")]
        public IActionResult Index()
        {
            var About = this._db.Abouts;
            ViewData["About"] = About;
            return View();
        }
        [HttpGet]
        [Route("Admin/About/Create")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Route("Admin/About/Create")]

        [ValidateAntiForgeryToken]
        public IActionResult Create(AboutCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string? FileName = this.UploadFile(vm.Image);
                string? LogoFileName = this.UploadFile(vm.Logo);
                About Obj = new About
                {
                    CompanyName = vm.CompanyName,
                    Logo = LogoFileName,
                    ShortDescription = vm.ShortDescription,
                    Description = vm.Description,
                    Image = FileName,
                    PhoneNnmber = vm.PhoneNnmber,
                    PhoneNumber2 = vm.PhoneNumber2,
                    Email = vm.Email,
                    Email2 = vm.Email2,
                    Address = vm.Address,
                    AddressLink = vm.AddressLink,
                    OfficeTime = vm.OfficeTime,
                    OffDay = vm.OffDay,
                    Vission = vm.Vission,
                    Mission = vm.Mission,
                    Strength = vm.Strength,
                };

                this._db.Abouts.Add(Obj);
                this._db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vm);
        }
        [HttpGet]
        [Route("Admin/About/Edit/{id}")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = this._db.Abouts.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            AboutEditViewModel obj = new AboutEditViewModel
            {
                Id = model.Id,
                CompanyName = model.CompanyName,
                ShortDescription = model.ShortDescription,
                Description = model.Description,
                PhoneNnmber = model.PhoneNnmber,
                PhoneNumber2 = model.PhoneNumber2,
                Email = model.Email,
                Email2 = model.Email2,
                Address = model.Address,
                AddressLink = model.AddressLink,
                OfficeTime = model.OfficeTime,
                OffDay = model.OffDay,
                ExistingImage = model.Image,
                ExistingLogo = model.Logo,
                Vission = model.Vission,
                Mission = model.Mission,
                Strength = model.Strength,

            };
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/About/Edit/{id}")]
        public IActionResult Edit(AboutEditViewModel EditModel)
        {

            if (ModelState.IsValid)
            {
                var Element = this._db.Abouts.Find(EditModel.Id);
                if (Element == null)
                {
                    return NotFound();
                }
                if (EditModel.Image != null)
                {
                    string NewFileName = this.UploadFile(EditModel.Image);
                    string fileLocation = Path.Combine(WebHostEnvironment.WebRootPath, "img/Gallary");
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
                if (EditModel.Logo != null)
                {
                    string NewFileName = this.UploadFile(EditModel.Logo);
                    string fileLocation = Path.Combine(WebHostEnvironment.WebRootPath, "img/Gallary");
                    if (EditModel.ExistingLogo != null)
                    {
                        string ExistingImagePath = Path.Combine(fileLocation, EditModel.ExistingLogo.Trim());
                        if (System.IO.File.Exists(ExistingImagePath))
                        {
                            System.IO.File.Delete(ExistingImagePath);
                        }
                    }
                    EditModel.ExistingLogo = NewFileName;
                }
                Element.CompanyName = EditModel.CompanyName;
                Element.ShortDescription = EditModel.ShortDescription;
                Element.Description = EditModel.Description;
                Element.PhoneNnmber = EditModel.PhoneNnmber;
                Element.PhoneNumber2 = EditModel.PhoneNumber2;
                Element.Email = EditModel.Email;
                Element.Email2 = EditModel.Email2;
                Element.Address = EditModel.Address;
                Element.AddressLink = EditModel.AddressLink;
                Element.OfficeTime = EditModel.OfficeTime;
                Element.OffDay = EditModel.OffDay;
                Element.Logo = EditModel.ExistingLogo;
                Element.Image = EditModel.ExistingImage;
                Element.Vission = EditModel.Vission;
                Element.Mission = EditModel.Mission;
                Element.Strength = EditModel.Strength;
                this._db.Abouts.Update(Element);
                this._db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(EditModel);
        }
        [Route("Admin/About/Delete/{id}")]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var Abt = this._db.Abouts.Find(id);
                if (Abt != null)
                {
                    if (Abt.Image != null)
                    {
                        string FileLocation = Path.Combine(WebHostEnvironment.WebRootPath, "img/Gallary");

                        string ExistingFilePath = Path.Combine(FileLocation, Abt.Image.Trim());
                        if (System.IO.File.Exists(ExistingFilePath))
                        {
                            System.IO.File.Delete(ExistingFilePath);
                        }
                    }
                    if (Abt.Logo != null)
                    {
                        string FileLocation = Path.Combine(WebHostEnvironment.WebRootPath, "img/Gallary");

                        string ExistingFilePath = Path.Combine(FileLocation, Abt.Logo.Trim());
                        if (System.IO.File.Exists(ExistingFilePath))
                        {
                            System.IO.File.Delete(ExistingFilePath);
                        }
                    }
                    this._db.Abouts.Remove(Abt);
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
                if (!Directory.Exists(Path.Combine(WebHostEnvironment.WebRootPath, "img/Gallary")))
                    Directory.CreateDirectory(Path.Combine(WebHostEnvironment.WebRootPath, "img/Gallary"));
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "img/Gallary");
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
