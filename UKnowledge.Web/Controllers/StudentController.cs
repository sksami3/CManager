using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CManager.Core.Entity;
using CManager.Core.Entity.AuthenticationModels;
using CManager.Core.Interfaces.Services;
using CManager.Web.DbContext;
using CManager.Web.Enums;
using CManager.Web.Models;
using CManager.Web.Models.ViewModels;

namespace CManager.Web.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IAttachmentsService _attachmentsService;
        private readonly IUserCourseService _userCourseService;
        public RoleManager<Role> _roleManager { get; }
        public UserManager<User> _userManager { get; }
        public StudentController( 
            RoleManager<Role> roleManager, 
            UserManager<User> userManager,
            ICourseService courseService,
            IAttachmentsService attachmentsService,
            IUserCourseService userCourseService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _courseService = courseService;
            _attachmentsService = attachmentsService;
            _userCourseService = userCourseService;
        }
        // GET: StudentController1
        public async Task<ActionResult> Index()
        {
            var courses = await _courseService.GetCourses();
            List<CourseViewModel> courseViewModels = Helper.Helper.ConvertCourseViweModelsFromCourses(courses.ToList(), User.Identity.Name);

            return View(courseViewModels);
        }
        public async Task<ActionResult> SavedCourses()
        {
            //var user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<Course> courses = await _userCourseService.GetCoursesCreatedByCurrentUser(User);//UserCourses.Where(x => x.User == user).Select(x=> x.Course).ToList();
            List<CourseViewModel> courseViewModels = Helper.Helper.ConvertCourseViweModelsFromCourses(courses, User.Identity.Name);

            return View(courseViewModels);
        }
        public async Task<ActionResult> CourseDetails(int courseId, string message = "")
        {
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }

            var course = await _courseService.GetCourseById(courseId);
            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (course == null)
                return NotFound();

            var courseViewModel = Helper.Helper.ConvertCourseViweModelFromCourse(course);
            #region Check if this course already saved or not
            if (course.UserCourses != null && course.UserCourses.Where(x => x.User == user).Count() > 0)
            {
                courseViewModel.IsCreatedByCurrentUser = true;
            }
            #endregion
            courseViewModel.Attachments = await _attachmentsService.GetAttachmentsByCourseId(course.Id);

            return View(courseViewModel);
        }
        public async Task<ActionResult> SaveTheCourse(int courseId)
        {
            try
            {
                var course = await _courseService.GetCourseById(courseId);
                var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
                UserCourse userCourse = new UserCourse()
                {
                    User = user,
                    Course = course,
                    CreatedBy = User.Identity.Name
                };
                await _userCourseService.Add(userCourse);

                return RedirectToAction("CourseDetails", "Student", new { courseId = courseId, message = "Saved successfully." });
            }
            catch (Exception e)
            {
                return RedirectToAction("CourseDetails", "Student", new { courseId = courseId, message = "Error occoured while saving." });
            }


        }

        public async Task<ActionResult> RemoveTheCourse(int courseId)
        {
            try
            {
                var course = await _courseService.GetCourseById(courseId);
                var user = _userManager.FindByNameAsync(User.Identity.Name).Result;

                UserCourse result = await _userCourseService.GetUserCourseByCourseAndUser(course,user);//UserCourses.Where(x => x.Course == course && x.User == user).SingleOrDefault();
                await _userCourseService.Remove(result.Id);

                return RedirectToAction("CourseDetails", "Student", new { courseId = courseId, message = "Removed successfully." });
            }
            catch (Exception e)
            {
                return RedirectToAction("CourseDetails", "Student", new { courseId = courseId, message = "Error occoured while saving." });
            }


        }
    }
}
