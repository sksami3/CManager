using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CManager.Core.Entity;
using CManager.Core.Entity.AuthenticationModels;
using CManager.Core.Interfaces.Repositories;
using CManager.Core.Interfaces.Services;

namespace CManager.Business.Services
{
    public class UserCourseService : IUserCourseService
    {
        private readonly IUserCourseReository _userCourseRepository;

        public UserCourseService(IUserCourseReository userCourseRepository)
        {
            _userCourseRepository = userCourseRepository;
        }

        public async Task Add(UserCourse userCourse)
        {
            await _userCourseRepository.AddAsync(userCourse);
            await _userCourseRepository.SaveChangesAsync();
        }

        public async Task<List<Course>> GetCoursesCreatedByCurrentUser(ClaimsPrincipal user)
        {
            return await _userCourseRepository.GetCoursesCreatedByCurrentUser(user);
        }

        public async Task<UserCourse> GetUserCourseByCourseAndUser(Course course, User user)
        {
            return await _userCourseRepository.GetUserCourseByCourseAndUser(course,user);
        }

        public async Task<UserCourse> GetUserCourseById(int id)
        {
            return await _userCourseRepository.FindAsync(id);
        }

        public async Task<IEnumerable<UserCourse>> GetUserCourses()
        {
            return await _userCourseRepository.All().ToListAsync();
        }

        public async Task Remove(int userCourseId)
        {
            await _userCourseRepository.Remove(userCourseId);
        }

        public async Task Update(UserCourse userCourse)
        {
            await _userCourseRepository.UpdateUserCourse(userCourse);
        }
    }
}
