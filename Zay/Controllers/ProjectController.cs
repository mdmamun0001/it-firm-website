using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Linq;
using Zay.Data;
using Zay.Models;
using Zay.ViewModel;
using System;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Zay.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly AplicationDbContext _db;
        private readonly IWebHostEnvironment WebHostEnvironment;
        public ProjectController(AplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            this._db = db;
            WebHostEnvironment = webHostEnvironment;
        }
        [Route("Admin/Project")]
        public IActionResult  Index()
        {
           ViewBag.projects = this._db.Projects.Include("Tools").OrderBy(s => s.DisplayOrder);
           return View();
        }
        [HttpGet]
        [Route("Admin/Project/Create")]
        public IActionResult Create()
        {
            List<Tool> tools = new List<Tool>();
            tools = this._db.Tools.ToList();
            ViewData["tools"] = tools;
            return View();
        }
        [HttpPost]
        [Route("Admin/Project/Create")]

        [ValidateAntiForgeryToken]
        public IActionResult Create(ProjectCreateViewModel vm)
        {
           
            if (ModelState.IsValid)
            {
                string? FileName = this.UploadFile(vm.Image);
                Project Obj = new Project
                {
                    Name = vm.Name,
                    ProjectUrl = vm.ProjectUrl,
                    ClientName = vm.ClientName,
                    BackgroundColor = vm.BackgroundColor,
                    ClientUrl = vm.ClientUrl,
                    Description = vm.Description,
                    Image = FileName,
                    CategoryName = vm.CategoryName,
                    Status = vm.Status,
                    PublishedDate = vm.PublishedDate,
                    DisplayOrder = vm.DisplayOrder,
                    IsActive = vm.IsActive,
                    Tools = new List<Tool>()
                };
                if (vm.ToolIds != null)
                {
                    foreach (var tool_id in vm.ToolIds)
                    {
                        var Toolmodel = this._db.Tools.Find(tool_id);
                        if (Toolmodel != null)
                        {
                            Obj.Tools.Add(Toolmodel);
                        }
                    }
                }
                this._db.Projects.Add(Obj);
                this._db.SaveChanges();
                return RedirectToAction("Index");
            }
           return View(vm);
        }
        [HttpGet]
        [Route("Admin/Project/Edit/{id}")]
        public IActionResult  Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = this._db.Projects.Include("Tools").Where(p => p.ProjectId == id).First();
            if (model == null)
            {
                return NotFound();
            }
            List<int> ExistingToolIds = new List<int>();
            if (model.Tools != null)
            {
                foreach (var item in model.Tools)
                {
                    ExistingToolIds.Add(item.ToolId);
                }
            }
            ProjectEditViewModel obj = new ProjectEditViewModel
            {
                ProjectId = model.ProjectId,
                Name = model.Name,
                ProjectUrl = model.ProjectUrl,
                ClientName = model.ClientName,
                ClientUrl = model.ClientUrl,
                BackgroundColor = model.BackgroundColor,
                Description = model.Description,
                CategoryName = model.CategoryName,
                Status = model.Status,
                PublishedDate = model.PublishedDate,
                DisplayOrder = model.DisplayOrder,
                IsActive = model.IsActive,
                ExistingToolIds = ExistingToolIds,
                NewToolIds = ExistingToolIds,
                ExistingImage = model.Image,

            };
            List<Tool> tools = new List<Tool>();
            tools = this._db.Tools.ToList();
            ViewData["tools"] = tools;
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/Project/Edit/{id}")]
        public IActionResult   Edit(ProjectEditViewModel  EditModel)
        {
           

            if (ModelState.IsValid)
            {

                if (EditModel.Image != null)
                {
                    string NewFileName = this.UploadFile(EditModel.Image);
                    string fileLocation = Path.Combine(WebHostEnvironment.WebRootPath, "img/Project");
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

                var Element = this._db.Projects.Include("Tools").Where(p => p.ProjectId == EditModel.ProjectId).First();
                if (Element != null)
                {
                    Element.Name = EditModel.Name;
                    Element.ProjectUrl = EditModel.ProjectUrl;
                    Element.ClientName = EditModel.ClientName;
                    Element.ClientUrl = EditModel.ClientUrl;
                    Element.BackgroundColor = EditModel.BackgroundColor;
                    Element.Description = EditModel.Description;
                    Element.CategoryName = EditModel.CategoryName;
                    Element.Status  = EditModel.Status;
                    Element.PublishedDate = EditModel.PublishedDate;
                    Element.IsActive = EditModel.IsActive;
                    Element.DisplayOrder = EditModel.DisplayOrder;
                    Element.Image = EditModel.ExistingImage;
                  //  IEnumerable<int> ShouldRemoveIds = new List<int>();
                  //  IEnumerable<int> ShouldAddIds = new List<int>();
                    if (EditModel.ExistingToolIds != null && EditModel.NewToolIds != null)
                    {
                        this.RemoveToolsToProject(EditModel.ExistingToolIds, EditModel.NewToolIds, Element);
                        this.AddToolsToProject (EditModel.ExistingToolIds, EditModel.NewToolIds, Element);
                    }
                    else if(EditModel.ExistingToolIds == null && EditModel.NewToolIds != null)
                    {
                        this.AddToolsToProject(new List<int>(), EditModel.NewToolIds, Element);
                    }
                    else if (EditModel.ExistingToolIds != null && EditModel.ExistingToolIds.Count() > 0 && EditModel.NewToolIds == null)
                    {
                        foreach (var item in EditModel.ExistingToolIds)
                        {
                            var RmvItm = this._db.Tools.Find(item);
                            Element.Tools.Remove(RmvItm);

                        }
                    }

                    this._db.Projects.Update(Element);
                    this._db.SaveChanges();
                    return RedirectToAction("Index");
                }
                 return NotFound();
            }
            return View(EditModel);
        }
        [Route("Admin/Project/Delete/{id}")]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var Prjct = this._db.Projects.Find(id);
                if (Prjct != null)
                {
                    if (Prjct.Image != null)
                    {
                        string FileLocation = Path.Combine(WebHostEnvironment.WebRootPath, "img/Project");

                        string ExistingFilePath = Path.Combine(FileLocation, Prjct.Image.Trim());
                        if (System.IO.File.Exists(ExistingFilePath))
                        {
                            System.IO.File.Delete(ExistingFilePath);
                        }
                    }

                    this._db.Projects.Remove(Prjct);
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
                if (!Directory.Exists(Path.Combine(WebHostEnvironment.WebRootPath, "img/Project")))
                    Directory.CreateDirectory(Path.Combine(WebHostEnvironment.WebRootPath, "img/Project"));
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "img/Project");
                fileName = id + "_" + image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }

            return fileName;
        }
        public void  RemoveToolsToProject(List<int> ExistingToolIds, List<int> NewToolIds, Project Obj)
        {
           IEnumerable<int> ShouldRemoveIds = ExistingToolIds.Except(NewToolIds);
            if (ShouldRemoveIds != null && ShouldRemoveIds.Count() > 0)
            {
                foreach (var item in ShouldRemoveIds)
                {
                    var RmvItm = this._db.Tools.Find(item);
                    Obj.Tools.Remove(RmvItm);

                }
            }
        }
        public void AddToolsToProject(List<int> ExistingToolIds, List<int> NewToolIds, Project Obj)
        {
            IEnumerable<int> ShouldAddIds = NewToolIds.Except(ExistingToolIds);
            if (ShouldAddIds != null && ShouldAddIds.Count() > 0)
            {
                foreach (var item in ShouldAddIds)
                {
                    var AddItm = this._db.Tools.Find(item);
                    Obj.Tools.Add(AddItm);

                }
            }
        }
    }
}
