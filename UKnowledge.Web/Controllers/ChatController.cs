using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uknowledge.Business.Utilities;
using UKnowledge.Core.Entity.AuthenticationModels;
using UKnowledge.Core.Interfaces.Services;
using UKnowledge.Web.Socket;

namespace UKnowledge.Web.Controllers
{
    public class ChatController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IMessageService _messageService;
        private readonly IAttachmentsService _attachmentsService;
        private readonly IWebHostEnvironment _env;
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private Utilities _util;
        private UserManager<User> _userManager { get; }

        public ChatController(ICourseService courseService, IWebHostEnvironment env,
            IAttachmentsService attachmentsService,
            IHubContext<NotificationHub> notificationHubContext,
            UserManager<User> userManager,
            IConfiguration configuration,
            IMessageService messageService)
        { 
            _courseService = courseService;
            _attachmentsService = attachmentsService;
            _env = env;
            _notificationHubContext = notificationHubContext;
            _userManager = userManager;
            _messageService = messageService;
            _util = new Utilities(configuration.GetConnectionString("DefaultConnection"));
        }
        public async Task<IActionResult> Index()
        {
            return View(await _messageService.GetMessage());
        }
    }
}
