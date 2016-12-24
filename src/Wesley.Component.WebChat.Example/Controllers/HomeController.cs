using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Wesley.Component.WebChat.Hubs;
using Wesley.Component.WebChat.Models;
using Wesley.Component.WebChat.Data;
using Wesley.Component.WebChat.Core;
using Wesley.Component.WebChat.Extensions;
using Microsoft.AspNetCore.Http;

namespace Wesley.Component.WebChat.Example.Controllers
{
    public class HomeController : Controller
    {
        private IPostRepository _postRepository { get; set; }
        private IConnectionManager _connectionManager { get; set; }

        public HomeController(IPostRepository postRepository, IConnectionManager connectionManager)
        {
            _postRepository = postRepository;
            _connectionManager = connectionManager;
        }

        public IActionResult Index()
        {
            if (IsAuthorize()) {
                return View();
            }
            else
            {
                return Redirect("/Home/Login");
            }

        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult DoLogin(string username)
        {
            var result = new StatusResult();
            if (username == "felli".ToLower() || username.ToLower() == "wesley")
            {
                result.Status = 1;
                HttpContext.Session.Set<string>("UserName",username);
            }
            return Json(result);
        }

        [HttpGet]
        public List<Post> GetPosts()
        {
            return _postRepository.GetAll();
        }

        [HttpGet]
        public void ClearPosts() {
            _postRepository.ClearPost();
        }

        [HttpGet]
        public Post GetPost(int id)
        {
            return _postRepository.GetPost(id);
        }

        [HttpPost]
        public void AddPost(Post post)
        {
            post.UserName =HttpContext.Session.Get<string>("UserName");
            _postRepository.AddPost(post);
            _connectionManager.GetHubContext<ChatHub>().Clients.All.publishPost(post);
        }

        private bool IsAuthorize() {
            var result = false;
            var userName = HttpContext.Session.Get<string>("UserName");
            if (!string.IsNullOrWhiteSpace(userName))
            {
                result= true;
            }
            return result;
        }
    }
}
