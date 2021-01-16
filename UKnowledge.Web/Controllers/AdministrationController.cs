using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UKnowledge.Core.Entity.AuthenticationModels;
using UKnowledge.Web.Models.ViewModels;

namespace UKnowledge.Web.Controllers
{
    public class AdministrationController : Controller
    {
        public RoleManager<Role> _roleManager { get; }
        public UserManager<User> _userManager { get; }

        public AdministrationController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        // GET: AdministrationController
        public ActionResult CreateRole()
        {
            return View();
        }

        // GET: AdministrationController/Details/5
        [HttpPost]
        public async Task<ActionResult> CreateRole(RoleViewModel roleViewModel)
        {
            ViewBag.Message = "";
            if (string.IsNullOrEmpty(roleViewModel.Name))
            {
                ViewBag.Message = "Role name can't be null";
                return View();
            }
            var newRole = new Role { Name = roleViewModel.Name };
            var result = await _roleManager.CreateAsync(newRole);
            if (result.Succeeded)
            {
                ViewBag.Message = "Role Successfully Created";
                return View();
            }
            else
            {
                ViewBag.Message = "Role Successfully Created";
                return View();
            }
        }
        public ActionResult RoleList(string _message = "")
        {
            #region If Error in Delete
            if (!string.IsNullOrEmpty(_message))
            {
                ViewBag.Message = _message;
            }
            #endregion
            //initializing List of RoleViewModel
            List<RoleViewModel> roleViewModels = new List<RoleViewModel>();
            //get all roles
            var roleList = _roleManager.Roles.ToList();
            //converting roles(IdentityRoles) to RolesViewModel
            foreach (var role in roleList)
            {
                RoleViewModel roleViewModel = new RoleViewModel();
                roleViewModel.Id = role.Id.ToString();
                roleViewModel.Name = role.Name;

                roleViewModels.Add(roleViewModel);
            }
            return View(roleViewModels);
        }
        public async Task<ActionResult> EditRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                #region Converting role(IdentityRole)  to RoleViewModel
                RoleViewModel roleViewModel = new RoleViewModel();
                roleViewModel.Id = role.Id.ToString();
                roleViewModel.Name = role.Name;
                #endregion
                return View(roleViewModel);
            }
            else
            {
                ViewBag.Message = "No Role found";
                return View();
            }
        }
        [HttpPost]
        public async Task<ActionResult> EditRole(RoleViewModel roleViewModel)
        {
            var role = await _roleManager.FindByIdAsync(roleViewModel.Id);
            if (role != null)
            {
                role.Name = roleViewModel.Name;
                var result = await _roleManager.UpdateAsync(role);
                if(result.Succeeded)
                {
                    ViewBag.Message = "Role Successfully Edited";
                    return View();
                }
                else
                {
                    if(result.Errors.ToList().Count() != 0)
                    {
                        string _errors = "";
                        List<string> errors = result.Errors.Select(x=> x.Description).ToList();
                        foreach(string err in errors)
                        {
                            _errors += err + " | ";
                        }
                        ViewBag.Message = _errors;
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "Error occured during update";
                        return View();
                    }
                    
                }
            }
            else
            {
                ViewBag.Message = "No Role found";
                return View();
            }
        }
        public async Task<ActionResult> DeleteRole(string roleId)
        {
            string message = "";
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    message = "Role Successfully Deleted";
                    return RedirectToAction("RoleList", new { _message = message });
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
                        message = _errors;
                        return RedirectToAction("RoleList", new { _message = message });
                    }
                    else
                    {
                        message = "Error occured during delete";
                        return RedirectToAction("RoleList", new { _message = message });
                    }

                }
            }
            else
            {
                message = "No Role found";
                return RedirectToAction("RoleList", new { _message = message });
            }
        }
    }
}
