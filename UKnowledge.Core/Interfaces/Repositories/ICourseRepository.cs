using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using UKnowledge.Core.Entity;
using UKnowledge.Core.Entity.AuthenticationModels;
using Unkowledge.Core.Interfaces.Repositories.Base;

namespace Unkowledge.Core.Interfaces.Repositories
{
    public interface ICourseRepository : IBaseRepository<Course>
    {
        Task UpdateCourse(Course model);
        Task Remove(int courseId);
        Task<List<Course>> GetCoursesCreatedByCurrentUser(ClaimsIdentity user);
        Task<List<Course>> GetCoursesCreatedByCurrentUser(ClaimsPrincipal user);
    }
}
