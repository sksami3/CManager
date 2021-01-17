using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CManager.Core.Entity;
using CManager.Core.Entity.AuthenticationModels;

namespace CManager.Core.Interfaces.Services
{
    public interface IUserCourseService
    {
        Task Add(UserCourse userCourse);
        Task<IEnumerable<UserCourse>> GetUserCourses();
        Task Update(UserCourse userCourse);
        Task Remove(int userCourseId);
        Task<UserCourse> GetUserCourseById(int id);
        Task<UserCourse> GetUserCourseByCourseAndUser(Course course, User user);
        Task<List<Course>> GetCoursesCreatedByCurrentUser(ClaimsPrincipal user);
    }
}
