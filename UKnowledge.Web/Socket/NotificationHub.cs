using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CManager.Business.Utilities;
using CManager.Core.Entity;
using CManager.Core.Entity.AuthenticationModels;
using CManager.Core.Interfaces.Services;
using CManager.Web.DbContext;
using CManager.Web.Enums;

namespace CManager.Web.Socket
{
    public class NotificationHub : Hub
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<User> _userManager;
        private readonly IMessageService _messageService;
        private RoleManager<Role> _roleManager { get; }
        Utilities _util;
        public NotificationHub(IConfiguration configuration, 
            IHttpContextAccessor httpContext, 
            UserManager<User> userManager, 
            RoleManager<Role> roleManager,
            IMessageService messageService)
        {
            _httpContext = httpContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _messageService = messageService;
            _util = new Utilities(configuration.GetConnectionString("DefaultConnection"));

        }

        public string GetUserId()
        {
            var userId = _userManager.GetUserId(_httpContext.HttpContext.User);
            return userId;
        }
        public ClaimsPrincipal GetUser()
        {
            return _httpContext?.HttpContext?.User as ClaimsPrincipal;
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendAddNotification(string toUser, string method, string fromUser)
        {
            await Clients.User(toUser).SendAsync(method, "message", GetUser().Identity.Name);
        }

        #region Chat
        public async Task SendToRole(string roleName, string message)
        {
            try
            {
                var user = await _userManager.GetUserAsync(GetUser());

                if (!string.IsNullOrEmpty(message.Trim()))
                {
                    // Create and save message in database
                    var msg = new Message();
                    msg.FromUser = user;
                    msg.ToRole = await _roleManager.FindByNameAsync(roleName);

                    await _messageService.Add(msg);

                    // Broadcast the message
                    var users = _userManager.Users.ToList();

                    if (users.Count() != 0)
                    {
                        foreach (var u in users)
                            if (_userManager.GetRolesAsync(u).Result.FirstOrDefault() == roleName)
                                await Clients.User(u.UserName).SendAsync("newMessage", message,user.UserName);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<User>> GetUsers(string roleName)
        {
            return await _util.GetAllUserByRole(roleName);
        }

        public async Task<List<Message>> GetMessageHistory(string roleName)
        {
            List<Message> messageHistory = await _messageService.GetMessageHistoryByRoleName(roleName);
                
            return messageHistory;
        }

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        private string IdentityName
        {
            get { return Context.User.Identity.Name; }
        }

        private string GetDevice()
        {
            var device = Context.GetHttpContext().Request.Headers["Device"].ToString();
            if (!string.IsNullOrEmpty(device) && (device.Equals("Desktop") || device.Equals("Mobile")))
                return device;

            return "Web";
        }
        #endregion

    }
}
