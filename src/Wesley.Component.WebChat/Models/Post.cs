using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wesley.Component.WebChat.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }

        public Post(int id, string userName, string text)
        {
            Id = id;
            UserName = userName;
            Text = text;
        }

        public Post()
        {
        }
    }
}
