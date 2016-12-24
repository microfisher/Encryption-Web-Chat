using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wesley.Component.WebChat.Models;

namespace Wesley.Component.WebChat.Data
{
    public interface IMessageRepository
    {
        List<Message> GetMessage();
        void SendMessage(Message sms);
        void ClearMessage();
    }
}
