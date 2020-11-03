using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UKnowledge.Web.DbContext;
using UKnowledge.Web.Enums;
using UKnowledge.Web.Models;
using UKnowledge.Web.Models.AuthenticationModels;
using UKnowledge.Web.Models.ViewModels;

namespace UKnowledge.Web.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly UKnowledgeDbContext _context;
        public RoleManager<IdentityRole> _roleManager { get; }
        public UserManager<User> _userManager { get; }
        public StudentController(UKnowledgeDbContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        // GET: StudentController1
        public ActionResult Index()
        {
            List<Course> courses = _context.Courses.ToList();
            List<CourseViewModel> courseViewModels = Helper.Helper.ConvertCourseViweModelsFromCourses(courses, User.Identity.Name);

            return View(courseViewModels);
        }
        public ActionResult SavedCourses()
        {
            User user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            List<Course> courses = _context.UserCourses.Where(x => x.User == user).Select(x=> x.Course).ToList();
            List<CourseViewModel> courseViewModels = Helper.Helper.ConvertCourseViweModelsFromCourses(courses, User.Identity.Name);

            return View(courseViewModels);
        }
        public ActionResult CourseDetails(int? courseId, string message = "")
        {
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            if (courseId == null)
            {
                return NotFound();
            }
            var course = _context.Courses.FirstOrDefault(m => m.Id == courseId);
            User user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            if (course == null)
                return NotFound();

            var courseViewModel = Helper.Helper.ConvertCourseViweModelFromCourse(course);
            #region Check if this course already saved or not
            if (course.UserCourses != null && course.UserCourses.Where(x => x.User == user).Count() > 0)
            {
                courseViewModel.IsCreatedByCurrentUser = true;
            }
            #endregion
            courseViewModel.Attachments = _context.Attachments.Where(x => x.CourseId == course.Id).ToList();

            return View(courseViewModel);
        }
        public ActionResult SaveTheCourse(int? courseId)
        {
            try
            {
                var course = _context.Courses.Where(x => x.Id == courseId).SingleOrDefault();
                var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
                UserCourse userCourse = new UserCourse()
                {
                    User = user,
                    Course = course
                };
                _context.UserCourses.Add(userCourse);
                _context.SaveChanges();

                return RedirectToAction("CourseDetails", "Student", new { courseId = courseId, message = "Saved successfully." });
            }
            catch (Exception e)
            {
                return RedirectToAction("CourseDetails", "Student", new { courseId = courseId, message = "Error occoured while saving." });
            }


        }

        public ActionResult RemoveTheCourse(int? courseId)
        {
            try
            {
                var course = _context.Courses.Where(x => x.Id == courseId).SingleOrDefault();
                var user = _userManager.FindByNameAsync(User.Identity.Name).Result;

                var result = _context.UserCourses.Where(x => x.Course == course && x.User == user).SingleOrDefault();
                _context.UserCourses.Remove(result);
                _context.SaveChanges();

                return RedirectToAction("CourseDetails", "Student", new { courseId = courseId, message = "Removed successfully." });
            }
            catch (Exception e)
            {
                return RedirectToAction("CourseDetails", "Student", new { courseId = courseId, message = "Error occoured while saving." });
            }


        }
    }
}
