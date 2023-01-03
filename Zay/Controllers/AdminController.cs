using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zay.Data;
using Zay.Models;

namespace Zay.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly AplicationDbContext _db;
        private readonly IWebHostEnvironment WebHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signinManager;
        public AdminController(AplicationDbContext db, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signinManager)
        {
            this._db = db;
            this.WebHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
            this.signinManager = signinManager;
            this.roleManager = roleManager;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
