using Inventory.Core.Exception;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CManager.Core.Entity;
using CManager.Core.Exception;
using CManager.Web.DbContext;
using CManager.Core.Interfaces.Repositories;
using CManager.Data.Repositories.Base;

namespace CManager.Data.Repositories
{
    public class CouseRepository : BaseRepository<Course>, ICourseRepository
    {
        private CManagerDbContext _context;
        public CouseRepository(CManagerDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task UpdateCourse(Course model)
        {
            var course = await Course(model.Id);

            course.Lectures = model.Lectures;
            course.Title = model.Title;
            course.TutorName = model.TutorName;
            course.UserCourses = model.UserCourses;

            Update(course);
            await SaveChangesAsync();
        }

        public async Task Remove(int courseId)
        {
            var course = await Course(courseId);

            Delete(course);
            await SaveChangesAsync();
        }
        public async Task<List<Course>> GetCoursesCreatedByCurrentUser(ClaimsIdentity user)
        {
            return await All(x => x.CreatedBy == user.Claims.ToString()).ToListAsync();
        }
        private async Task<Course> Course(int courseId)
        {
            var course = await FindAsync(courseId);
            if (course == null)
                throw new GenericException(Exceptions.CourseNotFound);
            return course;
        }

        public async Task<List<Course>> GetCoursesCreatedByCurrentUser(ClaimsPrincipal user)
        {
            var result = _context.Set<Course>().Where(x => x.CreatedBy == user.Identity.Name);
            return await result.ToListAsync();
        }
    }
}
