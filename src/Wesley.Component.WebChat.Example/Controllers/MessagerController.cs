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
    public class MessagerController : Controller
    {
        private IMessageRepository _messageRepository { get; set; }
        private IConnectionManager _connectionManager { get; set; }

        public MessagerController(IMessageRepository messageRepository, IConnectionManager connectionManager)
        {
            _messageRepository = messageRepository;
            _connectionManager = connectionManager;
        }

        public IActionResult Index()
        {
            if (IsAuthorize())
            {
                ViewData["UserName"] = HttpContext.Request.Cookies["UserName"].ToString();
                return View();
            }
            else
            {
                return Redirect("/Messager/Login");
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
        public JsonResult DoLogin(string username)
        {
            var result = new StatusResult();
            try
            {
                if (username == "felli".ToLower() || username.ToLower() == "wesley")
                {
                    result.Status = 1;
                    HttpContext.Response.Cookies.Append("UserName", username);
                }
            }
            catch (Exception ex)
            {
                result.Status = 0;
                result.Data = ex;
            }
            return Json(result);
        }

        /// <summary>
        /// 获取聊天记录
        /// </summary>
        /// <returns></returns>
        public List<Message> GetMessageList()
        {
            return _messageRepository.GetMessageList();
        }
        
        /// <summary>
        /// 移除已读消息
        /// </summary>
        /// <param name="guid"></param>
        public JsonResult UpdateStatus(string guid)
        {
            var result = new StatusResult(1, null);
            _messageRepository.UpdateStatus(guid);
            return Json(result);
        }

        /// <summary>
        /// 发送消息 
        /// </summary>
        /// <param name="content"></param>
        public JsonResult SendMessage(string content)
        {
            var result = new StatusResult();
            try
            {
                var guid = Guid.NewGuid().ToString();
                var userName = HttpContext.Request.Cookies["UserName"].ToString();
                var message = new Message(guid, new Account
                {
                    UserName = userName
                }, new Account{}, content, GetTime(), _messageRepository.MaxSequence);
                _messageRepository.SendMessage(message);
                _connectionManager.GetHubContext<ChatHub>().Clients.All.OnMessage(message);
                result.Status = 1;
            }
            catch (Exception ex)
            {
                result.Status = 0;
                result.Data = ex;
            }
            return Json(result);
        }

        /// <summary>
        /// 身份验证
        /// </summary>
        /// <returns></returns>
        private bool IsAuthorize()
        {
            var userName = HttpContext.Request.Cookies["UserName"].ToString();
            if (!string.IsNullOrWhiteSpace(userName))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        public double GetTime()
        {
            var span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return Math.Floor(span.TotalSeconds);
        }
    }

}
