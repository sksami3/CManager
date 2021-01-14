using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UKnowledge.Core.Entity.AuthenticationModels;

namespace UKnowledge.Web.Socket
{
    public class NotificationHub : Hub
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<User> _userManager;
        public NotificationHub(IHttpContextAccessor httpContext, UserManager<User> userManager)
        {
            _httpContext = httpContext;
            _userManager = userManager;
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

        public async Task SendAddNotification(string toUser,string method, string fromUser)
        {
            await Clients.User("ff1").SendAsync("add", "ff", GetUser().Identity.Name);
        }

    }
}
