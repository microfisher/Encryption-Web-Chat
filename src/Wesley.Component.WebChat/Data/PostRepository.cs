using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wesley.Component.WebChat.Models;

namespace Wesley.Component.WebChat.Data
{
    public class PostRepository : IPostRepository
    {
        private List<Post> _posts = new List<Post>();
        public void AddPost(Post post)
        {
            _posts.Add(post);
        }

        public List<Post> GetAll()
        {
            return _posts;
        }

        public Post GetPost(int id)
        {
            return _posts.FirstOrDefault(p => p.Id == id);
        }

        public void ClearPost() {
            _posts.Clear();
        }
    }
}
