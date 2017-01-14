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

namespace Wesley.Component.WebChat.Api.Controllers
{
    [Route("api/[controller]")]
    public class MessagerController : Controller
    {
        private IMessageRepository _messageRepository { get; set; }
        private IConnectionManager _connectionManager { get; set; }

        public MessagerController(IMessageRepository messageRepository, IConnectionManager connectionManager)
        {
            _messageRepository = messageRepository;
            _connectionManager = connectionManager;
        }

        /// <summary>
        /// 登录操作
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DoLogin(string username)
        {
            var result = new StatusResult();
            if (username == "felli".ToLower() || username.ToLower() == "wesley")
            {
                result.Status = 1;
                HttpContext.Session.Set<string>("UserName", username);
            }
            return Json(result);
        }

        /// <summary>
        /// 获取聊天记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Message> Get()
        {
            return _messageRepository.GetMessage();
        }

        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public double GetCurrentTime()
        {
            var span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return Math.Floor(span.TotalSeconds);
        }

        /// <summary>
        /// 移除已读消息
        /// </summary>
        /// <param name="guid"></param>
        [HttpGet]
        public void SetStatus(string guid)
        {
            _messageRepository.SetStatus(guid);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="content"></param>
        [HttpGet]
        public void SendMessage(string content)
        {
            var guid = Guid.NewGuid().ToString();
            var message = new Message(guid, new Account
            {
                UserName = HttpContext.Session.Get<string>("UserName")
            }, new Account
            {

            }, content, GetCurrentTime(), _messageRepository.MaxSequence);

            _messageRepository.SendMessage(message);
            _connectionManager.GetHubContext<ChatHub>().Clients.All.OnMessage(message);
        }

        ///// <summary>
        ///// 身份验证
        ///// </summary>
        ///// <returns></returns>
        //private bool IsAuthorize()
        //{
        //    var userName = HttpContext.Session.Get<string>("UserName");
        //    if (!string.IsNullOrWhiteSpace(userName))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //// GET api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
