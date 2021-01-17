using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using CManager.Business.Services;
using CManager.Data.Repositories;
using CManager.Core.Interfaces.Repositories;
using CManager.Core.Interfaces.Services;
using CManager.Business.Services;

namespace CManager.Web
{
    public static class ServiceResolver
    {
        public static void Resolve(this IServiceCollection services)
        {

            //services.AddTransient<IUnitOfWork, UnitOfWork>();

            #region service 
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<IAttachmentsService, AttachmentsService>();           
            services.AddTransient<IUserCourseService, UserCourseService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<IRoleService, RoleService>();
            #endregion

            #region repositories
            services.AddTransient<ICourseRepository, CouseRepository>();
            services.AddTransient<IAttachmentRepository, AttachmentRepository>();
            services.AddTransient<IUserCourseReository, UserCourseRepository>();
            services.AddTransient<IMessageRepositoy, MessageRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            #endregion







        }
    }
}
