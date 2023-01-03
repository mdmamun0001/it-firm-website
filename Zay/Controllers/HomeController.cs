using MailKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Zay.CustomHelper;
using Zay.Data;
using Zay.Models;
using Zay.ViewModel;
using Zay.Services;
using IMailService = Zay.Services.IMailService;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Zay.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AplicationDbContext _db;
        private readonly IWebHostEnvironment WebHostEnvironment;
        private readonly IMailService mailService;
        private  string CompanyName ;
        public HomeController(ILogger<HomeController> logger, AplicationDbContext db, IWebHostEnvironment webHostEnvironment, IMailService mailService)
        {
            _logger = logger;
            this._db = db;
            this.WebHostEnvironment = webHostEnvironment;
            this.mailService = mailService;
            this.CompanyName = "Zdm Information Technology";
        }

        public IActionResult Index()
        {
            ViewBag.ServiceCategories = this._db.ServiceCategories.Where(s => s.IsActive == true).OrderBy(s => s.DisplayOrder).ToList();
            ViewBag.Sliders = this._db.Sliders.Where(s => s.IsActive == true).OrderBy(s => s.DisplayOrder).ToList();
            ViewBag.Projects = this._db.Projects.Include("Tools").Where(p => p.IsActive == true).OrderBy(p => p.DisplayOrder).Take(3).ToList();
            
            ViewBag.Clients = this._db.Clients.Where(c => c.IsActive == true).OrderBy(c => c.DisplayOrder).ToList();
            ViewBag.Reviews = this._db.CustomerReviews.Where(c => c.IsActive == true).OrderBy(c => c.DisplayOrder).ToList();
            ViewBag.Tools = this._db.Tools.Where(t => t.IsActive == true).OrderBy(t => t.DisplayOrder).ToList(); 
            ViewBag.CompanyInfo = this._db.Abouts.ToList();
            ViewBag.SocialMedias = this._db.socialMedias.Where(s => s.IsActive == true).OrderBy(s => s.DisplayOrder).ToList();
            ViewBag.PhotoGallary = this._db.gallaries.Where(g => g.IsActive == true).OrderBy(g => g.DisplayOrder).ToList();
            ViewBag.ActiveNav = "Home";
            return View();
        }
        public IActionResult Protfolio()
        {
            ViewBag.ServiceCategories = this._db.ServiceCategories.Where(s => s.IsActive == true).OrderBy(s => s.DisplayOrder).ToList();
            ViewBag.Projects = this._db.Projects.Include("Tools").Where(p => p.IsActive == true).OrderBy(p => p.DisplayOrder).ToList();
            ViewBag.CompanyInfo = this._db.Abouts.ToList();
            ViewBag.SocialMedias = this._db.socialMedias.Where(s => s.IsActive == true).OrderBy(s => s.DisplayOrder).ToList();
            ViewBag.ActiveNav = "Protfolio";
            return View();
        }
        public IActionResult About()
        {
            ViewBag.ServiceCategories = this._db.ServiceCategories.Where(s => s.IsActive == true).OrderBy(s => s.DisplayOrder).ToList();
            ViewBag.CompanyInfo  = this._db.Abouts.ToList();
            ViewBag.SocialMedias = this._db.socialMedias.Where(s => s.IsActive == true).OrderBy(s => s.DisplayOrder).ToList();
            ViewBag.PhotoGallary = this._db.gallaries.Where(g => g.IsActive == true).OrderBy(g => g.DisplayOrder).ToList();
            ViewBag.Employes     = this._db.Employes.Where(e => e.IsActive == true).OrderBy(e => e.DisplayOrder).ToList();
            ViewBag.ActiveNav = "About";
            return View();
        }
        public IActionResult Services(int Id)
        {
            ViewBag.ServiceCategories = this._db.ServiceCategories.Where(s => s.IsActive == true).OrderBy(s => s.DisplayOrder).ToList();
            ViewBag.ServiceCategory = this._db.ServiceCategories.Include("Services").Where( S => S.Id == Id).First();
            ViewBag.CompanyInfo = this._db.Abouts.ToList();
            ViewBag.SocialMedias = this._db.socialMedias.Where(s => s.IsActive == true).OrderBy(s => s.DisplayOrder).ToList();
            ViewBag.ActiveNav = "Services";
            return View();
        }
        public IActionResult Project(int Id)
        {
            ViewBag.ServiceCategories = this._db.ServiceCategories.Where(s => s.IsActive == true).OrderBy(s => s.DisplayOrder).ToList();
            ViewBag.Project = this._db.Projects.Include("Tools").Where(P => P.ProjectId == Id).Where(P => P.IsActive == true).First();
            ViewBag.Reviews = this._db.CustomerReviews.Where(c => c.IsActive == true).OrderBy(c => c.DisplayOrder).ToList(); 
            ViewBag.CompanyInfo = this._db.Abouts.ToList();
            ViewBag.SocialMedias = this._db.socialMedias.Where(s => s.IsActive == true).OrderBy(s => s.DisplayOrder).ToList();
            ViewBag.Employes = this._db.Employes.Where(e => e.IsActive == true).OrderBy(e => e.DisplayOrder).ToList();
            ViewBag.ActiveNav = "others";
            return View();
        }
        public IActionResult Contact()
        {
            ViewBag.ServiceCategories = this._db.ServiceCategories.Where(s => s.IsActive == true).OrderBy(s => s.DisplayOrder).ToList();
            ViewBag.CompanyInfo = this._db.Abouts.ToList();
            ViewBag.SocialMedias = this._db.socialMedias.Where(s => s.IsActive == true).OrderBy(s => s.DisplayOrder).ToList();
            ViewBag.Clients = this._db.Clients.Where(c => c.IsActive == true).OrderBy(c => c.DisplayOrder).ToList();
            ViewBag.Reviews = this._db.CustomerReviews.Where(c => c.IsActive == true).OrderBy(c => c.DisplayOrder).ToList(); 
            ViewBag.ActiveNav = "Contact";
            return View();
        }
        [HttpPost]
        public  IActionResult Message(CustomerMessageCreateViewModel NewMessage)
        {
            if (ModelState.IsValid)
            {
                
                CustomerMessage CstmrMessage = new CustomerMessage
                {
                    Name = NewMessage.Name,
                    Email = NewMessage.Email,
                    PhoneNumber = NewMessage.PhoneNumber,
                    Message = NewMessage.Message,
                    IsSeen = false

                };
                this._db.CustomerMessages.Add(CstmrMessage);
                this._db.SaveChanges();
                TempData["FlashMessage"] = "Your message has been sent Successfully! We contact with you within 24 hours.";
                TempData["FlashMessageClass"] = "alert-success";

                var MailToAddresses = this._db.MailAddresses.Where(m => m.IsActive == true).ToList();
                if (MailToAddresses != null && MailToAddresses.Count > 0)
                {
                    List<string> mailingList = new List<string>();
                    foreach (var item in MailToAddresses)
                    {
                        mailingList.Add(item.Email);
                    }
                    ViewBag.CompanyInfo = this._db.Abouts.ToList();
                    if (ViewBag.CompanyInfo != null && ViewBag.CompanyInfo.Count > 0)
                    {
                        this.CompanyName = ViewBag.CompanyInfo[0].CompanyName;
                    }
                    MailRequest mailrequest = new MailRequest
                    {
                        Subject = "Customer Message At " + this.CompanyName,
                        Body = "<html> </body>" + "<h5>" + "Name: " + CstmrMessage.Name + "</h5>" + "<h5>" + " Email: " + CstmrMessage.Email + "</h5>" + "<h5>" + "Phone: "
                        + CstmrMessage.PhoneNumber + "</h5>" + "<p>" + CstmrMessage.Message + "</p>" + "</body></html>"


                    };
                    try
                    {

                         this.mailService.SendEmailAsync(mailrequest, mailingList).ConfigureAwait(false);

                        return RedirectToAction("Contact");
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Contact");
                    }
                }
                return RedirectToAction("Contact");
            }
            return RedirectToAction("Contact");
        }
        public IActionResult Appoinment()
        {
            ViewBag.ServiceCategories = this._db.ServiceCategories.Where(s => s.IsActive == true).OrderBy(s => s.DisplayOrder).ToList();
            ViewBag.CompanyInfo = this._db.Abouts.ToList();
            ViewBag.SocialMedias = this._db.socialMedias.Where(s => s.IsActive == true).OrderBy(s => s.DisplayOrder).ToList();
            ViewBag.Clients = this._db.Clients.Where(c => c.IsActive == true).OrderBy(c => c.DisplayOrder).ToList();
            ViewBag.Reviews = this._db.CustomerReviews.Where(c => c.IsActive == true).OrderBy(c => c.DisplayOrder).ToList();
            ViewBag.ActiveNav = "Contact";
            return View();
        }
        public IActionResult AppoinmentRequest(AppoinmentRequestCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {

                AppoinmentRequest ApnmntRequest = new AppoinmentRequest
                {
                    Name = vm.Name,
                    Email = vm.Email,
                    PhoneNumber = vm.PhoneNumber,
                    Message = vm.Message,
                    CustomerFlexibleDateTime = vm.CustomerFlexibleDateTime,
                    ServiceType = vm.ServiceType,
                    IsSeen = false

                };
                this._db.AppoinmentRequests.Add(ApnmntRequest);
                this._db.SaveChanges();
                TempData["FlashMessage"] = "Appoinment request has been sent Successfully! We will confirm meeting with you within 24 hours.";
                TempData["FlashMessageClass"] = "alert-success";

                var MailToAddresses = this._db.MailAddresses.Where(m => m.IsActive == true).ToList();
                if (MailToAddresses != null && MailToAddresses.Count > 0)
                {
                    List<string> mailingList = new List<string>();
                    foreach (var item in MailToAddresses)
                    {
                        mailingList.Add(item.Email);
                    }
                    ViewBag.CompanyInfo = this._db.Abouts.ToList();
                    if (ViewBag.CompanyInfo != null && ViewBag.CompanyInfo.Count > 0)
                    {
                        this.CompanyName = ViewBag.CompanyInfo[0].CompanyName;
                    }
                    MailRequest mailrequest = new MailRequest
                    {
                        Subject = "Appoinment Request At " + this.CompanyName,
                        Body = "<html> </body>" + "<h5>" + "Name: " + ApnmntRequest.Name + "</h5>" + "<h5>" + " Email: " + ApnmntRequest.Email + "</h5>" + "<h5>" + "Phone: "
                      + ApnmntRequest.PhoneNumber + "</h5>" + "<h5>" + "Service Type: " + ApnmntRequest.ServiceType + "</h5>" + "<h5>" + "Flexible Date Time: " + ApnmntRequest.CustomerFlexibleDateTime + "</h5>" + "<p>" + ApnmntRequest.Message + "</p>" + "</body></html>"

                    };
                    try
                    {

                          this.mailService.SendEmailAsync(mailrequest, mailingList).ConfigureAwait(false);

                        return RedirectToAction("Appoinment");
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Appoinment");
                    }
                }
                return RedirectToAction("Appoinment");
            }
            return RedirectToAction("Appoinment");
        }
        [HttpGet]
        public IActionResult JobApplication()
        {
            ViewBag.ServiceCategories = this._db.ServiceCategories.Where(s => s.IsActive == true).OrderBy(s => s.DisplayOrder).ToList();
            ViewBag.CompanyInfo = this._db.Abouts.ToList();
            ViewBag.SocialMedias = this._db.socialMedias.Where(s => s.IsActive == true).OrderBy(s => s.DisplayOrder).ToList();
            ViewBag.Clients = this._db.Clients.Where(c => c.IsActive == true).OrderBy(c => c.DisplayOrder).ToList();
            ViewBag.Reviews = this._db.CustomerReviews.Where(c => c.IsActive == true).OrderBy(c => c.DisplayOrder).ToList();
            ViewBag.Employes = this._db.Employes.Where(e => e.IsActive == true).OrderBy(e => e.DisplayOrder).ToList();
            ViewBag.ActiveNav = "Career";
            return View();
        }
        [HttpPost]
        public ActionResult JobApplication(JobApplicationCreateViewModel vm)
        {
                string? CvFileName = Helper.UploadFile(vm.Cv, "Storage/Cv", this.WebHostEnvironment);

                if (CvFileName == null)
                {

                    return RedirectToAction("JobApplication");
                }
                string? CoverLetterFileName = null;
                if (vm.CoverLetter != null)
                {

                     CoverLetterFileName = Helper.UploadFile(vm.CoverLetter, "Storage/CoverLetter", this.WebHostEnvironment);
                }

                JobApplication jobapply = new JobApplication
                {
                    Name = vm.Name,
                    Email = vm.Email,
                    PhoneNumber = vm.PhoneNumber,
                    Message = vm.Message,
                    JobTitle = vm.JobTitle,
                    Cv = CvFileName,
                    CoverLetter = CoverLetterFileName,
                    IsSeen = false

                };
                this._db.JobApplications.Add(jobapply);
                this._db.SaveChanges();
              
                TempData["FlashMessage"] = "Your application has been submitted Successfully! ";
                TempData["FlashMessageClass"] = "alert-success";
            var MailToAddresses = this._db.MailAddresses.Where(m => m.IsActive == true).ToList();
            if (MailToAddresses != null && MailToAddresses.Count > 0)
            {
                List<string> mailingList = new List<string>();
                foreach (var item in MailToAddresses)
                {
                    mailingList.Add(item.Email);
                }
                ViewBag.CompanyInfo = this._db.Abouts.ToList();
                if (ViewBag.CompanyInfo != null && ViewBag.CompanyInfo.Count > 0)
                {
                    this.CompanyName = ViewBag.CompanyInfo[0].CompanyName;
                }
                List<IFormFile> Attachment = new List<IFormFile>();
                if (vm.Cv != null)
                {
                    Attachment.Add(vm.Cv);
                }
                if (vm.CoverLetter != null)
                {
                    Attachment.Add(vm.CoverLetter);
                }
                MailRequest mailrequest = new MailRequest
                {
                    Subject = jobapply.JobTitle + " At " + this.CompanyName,

                    Body = "<html> </body>" + "<h5>" + "Name: " + jobapply.Name + "</h5>" + "<h5>" + " Email: " + jobapply.Email + "</h5>" + "<h5>" + "Phone: "
                      + jobapply.PhoneNumber + "</h5>" + "<h5>" + "Job Title: " + jobapply.JobTitle + "</h5>" + "<p>" + jobapply.Message + "</p>" + "</body></html>",

                    Attachments = Attachment
                };
                try
                {

                    this.mailService.SendEmailAsync(mailrequest, mailingList).ConfigureAwait(false);

                    return RedirectToAction("JobApplication");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("JobApplication");
                }
            }
            return RedirectToAction("JobApplication");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}