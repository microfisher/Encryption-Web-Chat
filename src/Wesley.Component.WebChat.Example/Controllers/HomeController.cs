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

        public List<Message> GetMessage()
        {
            return _messageRepository.GetMessage();
        }

        public void ClearMesssage()
        {
            _messageRepository.ClearMessage();
        }

        public void SendMessage(string content)
        {
            var message = new Message(new Account {
                UserName= HttpContext.Session.Get<string>("UserName")
            },new Account {

            },content);

            _messageRepository.SendMessage(message);

            _connectionManager.GetHubContext<ChatHub>().Clients.All.OnMessage(message);
        }

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
