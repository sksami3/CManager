using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using CManager.Core.Entity;
using CManager.Core.Entity.AuthenticationModels;

namespace CManager.Web.DbContext
{
    public class CManagerDbContext : IdentityDbContext<
        User
        , Role
        , Guid
        , IdentityUserClaim<Guid>
        , IdentityUserRole<Guid>
        , IdentityUserLogin<Guid>
        , IdentityRoleClaim<Guid>
        , IdentityUserToken<Guid>
        >
    {
        public CManagerDbContext(DbContextOptions<CManagerDbContext> options) : base(options)
        {

        }
        public DbSet<Attachments> Attachments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region Many-to-Many relation 
            modelBuilder.Entity<UserCourse>()
                .HasKey(uc => new { uc.UserId, uc.CourseId });
            modelBuilder.Entity<UserCourse>()
                .HasOne(uc => uc.User)
                .WithMany(b => b.UserCourses)
                .HasForeignKey(uc => uc.UserId);
            modelBuilder.Entity<UserCourse>()
                .HasOne(uc => uc.Course)
                .WithMany(c => c.UserCourses)
                .HasForeignKey(uc => uc.CourseId);
            #endregion
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
