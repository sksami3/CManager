using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CManager.Core.Entity;
using CManager.Core.Entity.AuthenticationModels;
using CManager.Core.Interfaces.Repositories.Base;

namespace CManager.Core.Interfaces.Repositories
{
    public interface IUserCourseReository : IBaseRepository<UserCourse>
    {
        Task UpdateUserCourse(UserCourse model);
        Task Remove(int courseId);
        Task<UserCourse> GetUserCourseByCourseAndUser(Course course, User user);
        Task<List<Course>> GetCoursesCreatedByCurrentUser(ClaimsPrincipal user);
    }
}

