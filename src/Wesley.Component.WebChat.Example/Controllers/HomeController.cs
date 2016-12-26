using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Wesley.Component.WebChat.Hubs;
using Wesley.Component.WebChat.Models;
using Wesley.Component.WebChat.Data;
using Wesley.Component.WebChat.Core;
using Wesley.Component.WebChat.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Wesley.Component.WebChat.Example.Controllers
{
    public class HomeController : Controller
    {
        private IMessageRepository _messageRepository { get; set; }
        private IConnectionManager _connectionManager { get; set; }

        public HomeController(IMessageRepository messageRepository, IConnectionManager connectionManager)
        {
            _messageRepository = messageRepository;
            _connectionManager = connectionManager;
        }

        public IActionResult Index()
        {
            if (IsAuthorize()) {
                ViewData["UserName"] = HttpContext.Session.Get<string>("UserName");
                return View();
            }
            else
            {
                return Redirect("/Home/Login");
            }

        }

        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登录操作
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public IActionResult DoLogin(string username)
        {
            var result = new StatusResult();
            if (username == "felli".ToLower() || username.ToLower() == "wesley")
            {
                result.Status = 1;
                HttpContext.Session.Set<string>("UserName",username);
            }
            return Json(result);
        }

        /// <summary>
        /// 获取聊天记录
        /// </summary>
        /// <returns></returns>
        public List<Message> GetMessage()
        {
            return _messageRepository.GetMessage();
        }

        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        public double GetCurrentTime()
        {
            var span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return Math.Floor(span.TotalSeconds);
        }

        /// <summary>
        /// 清除聊天记录
        /// </summary>
        public void ClearMessage()
        {
            _messageRepository.ClearMessage();
        }

        public void RemoveMessage(string guid)
        {
            _messageRepository.ClearMessage();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="content"></param>
        public void SendMessage(string content)
        {
            var guid = Guid.NewGuid().ToString();
            var message = new Message(guid, new Account {
                UserName = HttpContext.Session.Get<string>("UserName")
            }, new Account {

            }, content, GetCurrentTime());

            _messageRepository.SendMessage(message);
            _connectionManager.GetHubContext<ChatHub>().Clients.All.OnMessage(message);
        }

        /// <summary>
        /// 身份验证
        /// </summary>
        /// <returns></returns>
        private bool IsAuthorize()
        {
            var userName = HttpContext.Session.Get<string>("UserName");
            if (!string.IsNullOrWhiteSpace(userName))
            {
                return true;
            }
            return false;
        }
    }
}
