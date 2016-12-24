using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wesley.Component.WebChat.Models;

namespace Wesley.Component.WebChat.Data
{
    public class MessageRepository : IMessageRepository
    {
        private List<Message> _sms = new List<Message>();

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
    }
}
