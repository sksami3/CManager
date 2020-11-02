using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UKnowledge.Web.Models.AuthenticationModels;
using UKnowledge.Web.Models.ViewModels;

namespace UKnowledge.Web.Controllers
{
    public class UserController : Controller
    {
        public UserManager<User> _userManager { get; }
        public SignInManager<User> _signInManager { get; }
        public RoleManager<IdentityRole> _roleManager { get; }

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
            ViewBag.roleList = new SelectList(_roleManager.Roles.ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SignUp(UserViewModel userViewModel)
        {
            ViewBag.roleList = new SelectList(_roleManager.Roles.ToList(), "Id", "Name");
            if (ModelState.IsValid)
            {
                User newUser = new User { Email = userViewModel.Email, UserName = userViewModel.UserName };
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        var role = await _roleManager.FindByIdAsync(userViewModel.RoleId);
                        
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
                                    string _errors = "";
                                    List<string> errors = resultForRole.Errors.Select(x => x.Description).ToList();
                                    foreach (string err in errors)
                                    {
                                        _errors += err + " | ";
                                    }
                                    ViewBag.Message = _errors;
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
                                string _errors = "";
                                List<string> errors = result.Errors.Select(x => x.Description).ToList();
                                foreach (string err in errors)
                                {
                                    _errors += err + " | ";
                                }
                                ViewBag.Message = _errors;
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
            else
            {
                ViewBag.Message = "Validation Error";
                return View();
            }
        }
    }
}
