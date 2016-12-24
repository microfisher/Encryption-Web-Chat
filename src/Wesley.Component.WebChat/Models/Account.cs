using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wesley.Component.WebChat.Models
{
    public class Account
    {
        public int UserId { get; set; }

        public int ConnectionId { get; set; }

        public int Gender { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string NickName { get; set; }


    }
}
