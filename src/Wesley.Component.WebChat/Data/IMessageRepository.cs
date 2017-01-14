using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wesley.Component.WebChat.Models;

namespace Wesley.Component.WebChat.Data
{
    public interface IMessageRepository
    {
        int MaxSequence { get; }
        List<Message> GetMessageList();
        void SendMessage(Message sms);
        void UpdateStatus(string guid);
    }
}
