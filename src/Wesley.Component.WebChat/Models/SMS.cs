using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wesley.Component.WebChat.Models
{
    public class SMS
    {
        public int FromUserId { get; set; }

        public int ToUserId { get; set; }

        public string Message { get; set; }

    }
}
