using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UKnowledge.Core.Entity;
using UKnowledge.Core.Entity.AuthenticationModels;

namespace UKnowledge.Core.Interfaces.Services
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
