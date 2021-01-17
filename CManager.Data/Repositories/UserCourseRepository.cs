using Inventory.Core.Exception;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CManager.Core.Entity;
using CManager.Core.Entity.AuthenticationModels;
using CManager.Core.Exception;
using CManager.Core.Interfaces.Repositories;
using CManager.Web.DbContext;
using CManager.Data.Repositories.Base;

namespace CManager.Data.Repositories
{
    public class UserCourseRepository : BaseRepository<UserCourse>, IUserCourseReository
    {
        private CManagerDbContext _context;
        public UserCourseRepository(CManagerDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task UpdateUserCourse(UserCourse model)
        {
            var userCourse = await UserCourse(model.Id);

            userCourse.UserId = model.UserId;
            userCourse.CourseId = model.CourseId;
            userCourse.User = model.User;
            userCourse.Course = model.Course;

            Update(userCourse);
            await SaveChangesAsync();
        }

        public async Task Remove(int userCourseId)
        {
            var userCourse = await UserCourse(userCourseId);

            Delete(userCourse);
            await SaveChangesAsync();
        }
        private async Task<UserCourse> UserCourse(int userCourseId)
        {
            var userCourse = await FindAsync(userCourseId);
            if (userCourse == null)
                throw new GenericException(Exceptions.UserCourseNotFound);
            return userCourse;
        }

        public async Task<UserCourse> GetUserCourseByCourseAndUser(Course course, User user)
        {
            var result = _context.Set<UserCourse>().Where(x => x.Course == course && x.User == user);
            return await result.SingleOrDefaultAsync();
        }

        public async Task<List<Course>> GetCoursesCreatedByCurrentUser(ClaimsPrincipal user)
        {
            var result = _context.Set<UserCourse>().Where(x => x.CreatedBy == user.Identity.Name).Select(x => x.Course);
            return await result.ToListAsync();
        }
    }
}
