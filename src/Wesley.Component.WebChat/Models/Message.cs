using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wesley.Component.WebChat.Models
{
    public class Message
    {
        public string Id { get; set; } = string.Empty;
             
        public Account FromUser { get; set; } = new Account();

        public Account ToUser { get; set; } = new Account();

        public string Content { get; set; } = string.Empty;

        public double SendTime { get; set; } = 0;

        public int ReadStatus { get; set; } = 0;

        public Message() {

        }

        public Message(string id, Account fromUser, Account toUser,string content,double sendTime)
        {
            Id = id;
            FromUser = fromUser;
            ToUser = toUser;
            Content = content;
            SendTime = sendTime;
        }
    }
}
