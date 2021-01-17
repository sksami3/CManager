using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CManager.Core.Entity;
using CManager.Core.Entity.AuthenticationModels;
using CManager.Web.Models;
using CManager.Web.Models.ViewModels;

namespace CManager.Web.Helper
{
    public static class Helper
    {
        public static string GetErrorList(IdentityResult result)
        {
            string _errors = "";
            List<string> errors = result.Errors.Select(x => x.Description).ToList();
            foreach (string err in errors)
            {
                _errors += err + " ";
            }
            return _errors;
        }

        public static List<CourseViewModel> ConvertCourseViweModelsFromCourses(List<Course> courses, string currentUser = "")
        {
            List<CourseViewModel> courseViewModels = new List<CourseViewModel>();
            foreach (Course c in courses)
            {
                CourseViewModel courseViewModel = new CourseViewModel();
                courseViewModel.Description = c.Description;
                courseViewModel.TutorName = c.TutorName;
                courseViewModel.Title = c.Title;
                courseViewModel.CourseId = c.Id;
                courseViewModel.CreatedBy = c.CreatedBy;
                if (currentUser.Equals(c.CreatedBy))
                    courseViewModel.IsCreatedByCurrentUser = true;
              
                courseViewModels.Add(courseViewModel);
            }
            return courseViewModels;
        }

        public static CourseViewModel ConvertCourseViweModelFromCourse(Course course)
        {
            CourseViewModel courseViewModel = new CourseViewModel();
            courseViewModel.CourseId = course.Id;
            courseViewModel.CreatedBy = course.CreatedBy;
            courseViewModel.Description = course.Description;
            courseViewModel.Title = course.Title;
            courseViewModel.TutorName = course.TutorName;

            return courseViewModel;
        }

        internal static Course ConvertCourseFromCourseViewModel(CourseViewModel courseViewModel)
        {
            Course course = new Course();
            course.Title = courseViewModel.Title;
            course.TutorName = courseViewModel.TutorName;
            course.CreatedDate = DateTime.Now;
            course.UpdatedDate = DateTime.Now;
            course.Description = courseViewModel.Description;

            return course;
        }
    }
}
