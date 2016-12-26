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
            foreach (var item in _sms) {
                item.SendTime = GetCurrentTime();
            }
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

        public double GetCurrentTime()
        {
            var span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return Math.Floor(span.TotalSeconds);
        }
    }
}
