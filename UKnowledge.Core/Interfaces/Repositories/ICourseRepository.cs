using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CManager.Core.Entity;
using CManager.Core.Entity.AuthenticationModels;
using CManager.Core.Interfaces.Repositories.Base;

namespace CManager.Core.Interfaces.Repositories
{
    public interface ICourseRepository : IBaseRepository<Course>
    {
        Task UpdateCourse(Course model);
        Task Remove(int courseId);
        Task<List<Course>> GetCoursesCreatedByCurrentUser(ClaimsIdentity user);
        Task<List<Course>> GetCoursesCreatedByCurrentUser(ClaimsPrincipal user);
    }
}
