using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zay.Data;
using Zay.Models;
using Zay.ViewModel;

namespace Zay.Controllers
{
    public class AccountController : Controller
    {
        private readonly AplicationDbContext _db;
        private readonly IWebHostEnvironment WebHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signinManager;
        public AccountController(AplicationDbContext db, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signinManager)
        {
            this._db = db;
            this.WebHostEnvironment = webHostEnvironment;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signinManager = signinManager;
        }
        [Route("Admin/Users")]
        [Authorize]
        public IActionResult Index()
        {
            ViewBag.AllUser = userManager.Users;
            return View();
        }
        [HttpGet]
        [Authorize]
        [Route("Admin/User/Edit/{id}")]
        public  async Task <IActionResult> Edit(string id)
        {
            if (id != null)
            {

                var User = await userManager.FindByIdAsync(id);
                if (User != null)
                {
                    UserEditViewModel EditModel = new UserEditViewModel {
                        Name = User.Name,
                        Id   = User.Id
                    };
                    return View(EditModel);
                }
                return NotFound();
            }
            return NotFound();
        }
        [HttpPost]
        [Route("Admin/User/Edit/{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(UserEditViewModel EditModel)
        {
            if (ModelState.IsValid)
            {

                var User = await userManager.FindByIdAsync(EditModel.Id);
                if (User != null && EditModel.NewPassword != null)
                {
                    User.Name = EditModel.Name;
                    User.PasswordHash =  userManager.PasswordHasher.HashPassword(User, EditModel.NewPassword);
                   var resultInfo = await userManager.UpdateAsync(User);
                 //  var resetToken = await userManager.GeneratePasswordResetTokenAsync(User);
                 //  var result = await userManager.ResetPasswordAsync(User, resetToken, EditModel.NewPassword);
                    if (resultInfo.Succeeded )
                    {

                        TempData["FlashMessage"] = "A user Information has been updated successfully.";
                        TempData["FlashMessageClass"] = "alert-success";
                        return RedirectToAction("Index");
                    }
                    foreach (var error in resultInfo.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                TempData["FlashMessage"] = "A user account not found.";
                TempData["FlashMessageClass"] = "alert-danger";
                return View();
            }
            return View();
        }
        [HttpGet]
        [Authorize]
        [Route("Admin/Register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        [Route("Admin/Register")]
        public async Task<IActionResult> Register(UserCreateViewModel vm)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser { Name=vm.Name, UserName = vm.UserEmail, Email = vm.UserEmail };
                var result = await userManager.CreateAsync(user, vm.Password);
                if (result.Succeeded)
                {
                 //  await signinManager.SignInAsync(user, isPersistent:false);

                    TempData["FlashMessage"] = "A user account has been created successfully.";
                    TempData["FlashMessageClass"] = "alert-success";
                    return RedirectToAction("Register");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }
        [HttpGet]
        [Route("Account/Login")]
        public async Task<IActionResult> Login()
        {
            if (userManager.Users.ToList().Count <1)
            { 
               var Result = await DumyData.Initialize(userManager);
                if (Result == false)
                {
                    ModelState.AddModelError(string.Empty, "Create User First");
                    return View();
                }
            }
            return View();
        }
        [HttpPost]
        [Route("Account/Login")]
        public async Task<IActionResult> Login(UserLoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var result = await signinManager.PasswordSignInAsync(vm.UserEmail, vm.Password, vm.RememberMe, false);

                if (result.Succeeded)
                {
                    return Redirect("/Admin/Index");
                }
                
                ModelState.AddModelError(string.Empty, "Invalid Attempt");
                
            }
            return View(vm);
        }
        [HttpGet]
        [Authorize]
        [Route("Account/Logout")]
        public async Task<IActionResult> Logout ()
        {
            await signinManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        [HttpGet]
        [Authorize]
       [Route("Admin/Account/Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id != null)
            {

                var user = await userManager.FindByIdAsync(id);
                if (user != null)
                {
                  var result =  await userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        string authId = userManager.GetUserId(User);
                        if (authId == id)
                        {
                            return RedirectToAction("Logout");
                        }
                        TempData["FlashMessage"] = "A user account has been Deleted successfully.";
                        TempData["FlashMessageClass"] = "alert-success";
                        return RedirectToAction("Index");
                    }
                    TempData["FlashMessage"] = "A user account deletion failed.";
                    TempData["FlashMessageClass"] = "alert-danger";
                    return RedirectToAction("Index");
                }
                TempData["FlashMessage"] = "A user account not found.";
                TempData["FlashMessageClass"] = "alert-danger";
                return RedirectToAction("Index");
            }
            TempData["FlashMessage"] = "A user account not found.";
            TempData["FlashMessageClass"] = "alert-danger";
            return RedirectToAction("Index");
        }
    }
}
