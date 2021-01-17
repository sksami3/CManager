using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CManager.Core.Entity.AuthenticationModels;
using CManager.Web.Enums;

namespace CManager.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public UserManager<User> _userManager { get; }
        public SignInManager<User> _signInManager { get; }
        public RoleManager<Role> _roleManager { get; }

        public HomeController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        // This is the starting method. Users will be redirected to their pages accourding to thir roles
        [AllowAnonymous]
        public ActionResult Index()
        {
            if(!_signInManager.IsSignedIn(User))
                return RedirectToAction("Login", "User");
            if (_signInManager.IsSignedIn(User) && User.IsInRole(RoleEnum.Staff.ToString()))
                return RedirectToAction("Index", "Staff");
            if (_signInManager.IsSignedIn(User) && User.IsInRole(RoleEnum.Student.ToString()))
                return RedirectToAction("Index", "Student");

            return RedirectToAction("Error", "Home");
        }
        [AllowAnonymous]
        public ActionResult Error()
        {
            return View();
        }
    }
}
