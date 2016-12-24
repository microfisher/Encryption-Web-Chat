using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wesley.Component.WebChat.Models
{
    public class Message
    {
        public Account FromUser { get; set; } = new Account();

        public Account ToUser { get; set; } = new Account();

        public string Content { get; set; } = string.Empty;

        public Message() {

        }
        public Message(Account fromUser, Account toUser,string content)
        {
            FromUser = fromUser;
            ToUser = toUser;
            Content = content;
        }
    }
}
