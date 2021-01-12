using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using UKnowledge.Core.Entity;
using UKnowledge.Core.Entity.AuthenticationModels;

namespace UKnowledge.Core.Interfaces.Services
{
    public interface ICourseService
    {
        Task Add(Course course);
        Task<IEnumerable<Course>> GetCourses();
        Task Update(Course course);
        Task Remove(int courseId);
        Task<Course> GetCourseById(int id);
        Task<List<Course>> GetCoursesCreatedByCurrentUser(ClaimsIdentity user);
        Task<List<Course>> GetCoursesCreatedByCurrentUser(ClaimsPrincipal user);
    }
}
