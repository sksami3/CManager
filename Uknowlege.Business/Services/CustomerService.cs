using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CManager.Core.Entity;
using CManager.Core.Entity.AuthenticationModels;
using CManager.Core.Interfaces.Services;
using CManager.Core.Interfaces.Repositories;

namespace CManager.Business.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task Add(Course course)
        {
            await _courseRepository.AddAsync(course);
            await _courseRepository.SaveChangesAsync();
        }

        public async Task<Course> GetCourseById(int id)
        {
            return await _courseRepository.FindAsync(id);
        }

        public async Task<IEnumerable<Course>> GetCourses()
        {
            return await _courseRepository.All().ToListAsync();
        }

        public async Task<List<Course>> GetCoursesCreatedByCurrentUser(ClaimsIdentity user)
        {
            return await _courseRepository.GetCoursesCreatedByCurrentUser(user);
        }

        public async Task<List<Course>> GetCoursesCreatedByCurrentUser(ClaimsPrincipal user)
        {
            return await _courseRepository.GetCoursesCreatedByCurrentUser(user);
        }

        public async Task Remove(int courseId)
        {
            await _courseRepository.Remove(courseId);
        }

        public async Task Update(Course course)
        {
            await _courseRepository.UpdateCourse(course);
        }
    }
}