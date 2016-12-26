using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wesley.Component.WebChat.Extensions;
using Wesley.Component.WebChat.Models;

namespace Wesley.Component.WebChat.Data
{
    public class MessageRepository : IMessageRepository
    {
        private List<Message> _sms = new List<Message>();

        private static IHttpContextAccessor _context { get; set; }

        public void SendMessage(Message sms)
        {
            _sms.Add(sms);
        }

        public List<Message> GetMessage()
        {
            return _sms;
        }

        public void ClearMessage() {
            _sms.Clear();
        }

        public void RemoveMessage(string guid)
        {
            var userName = _context.HttpContext.Session.Get<string>("UserName");
            var getMessage = _sms.Find(m => m.Id == guid && m.FromUser.UserName.ToLower()== userName.ToLower());
            if (getMessage != null && userName!=null) {
                _sms.Remove(getMessage);
            }
        }
    }
}
