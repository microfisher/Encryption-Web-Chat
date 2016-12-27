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

        public int MaxSequence {
            get {
                if (_sms.Count > 0)
                {
                    return _sms.Max(m => m.Sequence);
                }
                else {
                    return 1;
                }
            }
        }

        public void SendMessage(Message sms)
        {
            _sms.Add(sms);
        }

        public List<Message> GetMessage()
        {
            foreach (var item in _sms) {
                item.CreatedTime = GetCurrentTime();
            }
            return _sms;
        }

        public void SetStatus(string guid)
        {
            var message = _sms.Find(m => m.Id.ToLower() == guid.ToLower());
            if (message != null)
            {
                message.ReadStatus = 1;
            }
        }

        public double GetCurrentTime()
        {
            var span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return Math.Floor(span.TotalSeconds);
        }
    }
}
