using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wesley.Component.WebChat.Models;

namespace Wesley.Component.WebChat.Data
{
    public interface IPostRepository
    {
        List<Post> GetAll();
        Post GetPost(int id);
        void AddPost(Post post);
        void ClearPost();
    }
}
