using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using leodacosta02.Models.CommentModel;
using System.Web.Mvc;

namespace leodacosta02.Controllers.CommentController
{
    public class CommentController : Controller
    {
        // GET: Comment
        public ActionResult Index()
        {
            List<CommentModel> comments = new List<CommentModel>
            {
                new CommentModel
                {
                    CommentId = 1,
                    RepliesCount = 1,
                    User = "John",
                    Text = "This is a comment.",
                    CreatedAt = new DateTime(2024, 12, 11, 15, 23, 0),
                    Replies = new List<CommentModel>
                    {
                        new CommentModel
                        {
                            CommentId = 101,
                            RepliesCount = 0,
                            User = "Johnson",
                            Text = "This is a reply.",
                            CreatedAt = DateTime.Now
                        }
                    }
                },
                new CommentModel
                {
                    CommentId = 1,
                    RepliesCount = 1,
                    User = "Jack",
                    Text = "This is a comment.",
                    CreatedAt = new DateTime(2024, 12, 11, 14, 23, 0)
                },

            };
            return View(comments);
        }
    }
}