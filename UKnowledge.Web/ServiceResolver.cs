using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Uknowledge.Business.Services;
using Uknowledge.Data.Repositories;
using UKnowledge.Core.Interfaces.Repositories;
using UKnowledge.Core.Interfaces.Services;
using Uknowlege.Business.Services;
using Unkowledge.Core.Interfaces.Repositories;
using Unkowledge.Data.Repositories;

namespace UKnowledge.Web
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
            #endregion

            #region repositories
            services.AddTransient<ICourseRepository, CouseRepository>();
            services.AddTransient<IAttachmentRepository, AttachmentRepository>();
            services.AddTransient<IUserCourseReository, UserCourseRepository>();
            #endregion







        }
    }
}
