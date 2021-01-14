using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UKnowledge.Core.Entity.AuthenticationModels;
using UKnowledge.Web.Enums;
using UKnowledge.Web.Models.ViewModels;

namespace UKnowledge.Web.Controllers
{
    public class UserController : Controller
    {
        private UserManager<User> _userManager { get; }
        private SignInManager<User> _signInManager { get; }
        private RoleManager<IdentityRole> _roleManager { get; }

        public UserController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        // GET: UserController
        public ActionResult SignUp()
        {
            #region Dropdown
            List<IdentityRole> identityRoles = new List<IdentityRole>()
            {
                new IdentityRole(){
                    Name = RoleEnum.Staff.ToString()
                },
                new IdentityRole(){
                    Name = RoleEnum.Student.ToString()
                }
            };
            ViewBag.roleList = new SelectList(identityRoles, "Name", "Name");
            #endregion 
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SignUp(UserViewModel userViewModel)
        {
            #region Dropdown
            List<IdentityRole> identityRoles = new List<IdentityRole>()
            {
                new IdentityRole(){
                    Name = RoleEnum.Staff.ToString()
                },
                new IdentityRole(){
                    Name = RoleEnum.Student.ToString()
                }
            };
            ViewBag.roleList = new SelectList(identityRoles, "Name", "Name");
            #endregion
            User newUser = new User { Email = userViewModel.Email, UserName = userViewModel.UserName };
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    #region Here we'll find if the role is created if not the we'll create the role
                    var role = await _roleManager.FindByNameAsync(userViewModel.RoleId); //here role id is used to carry role name
                    if(role == null)
                    {
                        IdentityRole newRole = new IdentityRole();
                        newRole.Name = userViewModel.RoleId; //here role id is used to carry role name
                        await _roleManager.CreateAsync(newRole);
                        role = newRole;
                    }
                    #endregion
                    var result = await _userManager.CreateAsync(newUser, userViewModel.Password);
                    if (result.Succeeded)
                    {
                        var user = await _userManager.FindByIdAsync(newUser.Id);
                        var resultForRole = await _userManager.AddToRoleAsync(user, role.Name);
                        if (resultForRole.Succeeded)
                        {
                            scope.Complete();
                            ViewBag.Message = "Registered Successfully!!!";

                            return View();
                        }
                        else
                        {
                            if (resultForRole.Errors.ToList().Count() != 0)
                            {
                                ViewBag.Message = Helper.Helper.GetErrorList(resultForRole);
                                scope.Dispose();
                                return View();
                            }
                            else
                            {
                                ViewBag.Message = "Error occoured!!!";
                                scope.Dispose();
                                return View();
                            }
                        }
                    }
                    else
                    {
                        if (result.Errors.ToList().Count() != 0)
                        {
                            ViewBag.Message = Helper.Helper.GetErrorList(result); ;
                            scope.Dispose();
                            return View();
                        }
                        else
                        {
                            ViewBag.Message = "Error occoured!!!";
                            scope.Dispose();
                            return View();
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Message = "Error occoured!!!";
                    scope.Dispose();
                    return View();
                }
            }
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(UserViewModel userViewModel)
        {
            var user = await _userManager.FindByNameAsync(userViewModel.UsernameOrEmail) ?? await _userManager.FindByEmailAsync(userViewModel.UsernameOrEmail);
            if (user == null)
            {
                ViewBag.Message = "No user found with this username/email.";
                return View();
            }
            try
            {
                var result = await _signInManager.PasswordSignInAsync(user, userViewModel.Password, false, true);
                if (result.Succeeded)
                {
                    //as each user can have only one role, so we can take single
                    var role = (await _userManager.GetRolesAsync(user)).Single();
                    #region
                    var claims = new List<Claim>
                        {
                          new Claim(ClaimTypes.Name, user.UserName),
                          new Claim(ClaimTypes.Email, user.Email),
                          new Claim(ClaimTypes.Role, role)
                        };

                    var claimsIdentity = new ClaimsIdentity(
                      claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties();

                    await HttpContext.SignInAsync(
                      CookieAuthenticationDefaults.AuthenticationScheme,
                      new ClaimsPrincipal(claimsIdentity),
                      authProperties);
                    #endregion
                    if (role == RoleEnum.Staff.ToString())
                        return RedirectToAction("Index", "Staff");
                    else
                        return RedirectToAction("Index", "Student");
                }
                else
                {
                    ViewBag.Message = "Please enter correct password.";
                    return View();
                }

            }
            catch (Exception e)
            {
                ViewBag.Message = "Error occoured!!";
                return View();
                throw e;
            }
        }
        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Index", "Home");
        }
    }
}
