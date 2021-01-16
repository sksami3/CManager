using Inventory.Core.Exception;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UKnowledge.Core.Entity;
using UKnowledge.Core.Exception;
using UKnowledge.Web.DbContext;
using Unkowledge.Core.Interfaces.Repositories;
using Unkowledge.Data.Repositories.Base;

namespace Unkowledge.Data.Repositories
{
    public class CouseRepository : BaseRepository<Course>, ICourseRepository
    {
        private UKnowledgeDbContext _context;
        public CouseRepository(UKnowledgeDbContext context)
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
