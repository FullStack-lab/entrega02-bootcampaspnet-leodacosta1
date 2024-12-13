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

         private static List<CommentModel> comments = new List<CommentModel>
            {
                new CommentModel
                {
                    CommentId = 1,
                    User = "John",
                    Text = "This is a comment.",
                    CreatedAt = new DateTime(2024, 12, 11, 15, 23, 0),
                    Replies = new List<CommentModel>
                    {
                        new CommentModel
                        {
                            CommentId = 101,
                            User = "Johnson",
                            Text = "This is a reply.",
                            CreatedAt = DateTime.Now
                        },
                        new CommentModel
                        {
                            CommentId = 102,
                            User = "Jackson",
                            Text = "This is a 2nd reply.",
                            CreatedAt = DateTime.Now
                        }
                    }
                },
                new CommentModel
                {
                    CommentId = 1,
                    User = "Jack",
                    Text = "This is a comment.",
                    CreatedAt = new DateTime(2024, 12, 11, 14, 23, 0)
                },

            };
        public ActionResult Index()
        {
            return View(comments);
        }
        // página pra criação de novo comentário
        public ActionResult Create()
        {
            return View(new CommentModel());
        }

        // Método POST: Criação de novo comentário / post
        [HttpPost]
        public ActionResult Create(CommentModel comment)
        {
            if (ModelState.IsValid)
            {
                if (comment.ParentCommentId == 0) // se for igual a zero, não é um reply, e o código executa o post normal
                {
                    // Confere se a lista está vazia antes de usar .Max()
                    if (comments.Count == 0)
                    {
                        comment.CommentId = 1; // Se a lista está vazia, atribui o valor de 1 para um CommentId de um comentário
                    }
                    else
                    {
                        comment.CommentId = comments.Max(t => t.CommentId) + 1; // Incrementa em 1 com base no valor máximo da lista
                    }
                    comment.CreatedAt = DateTime.Now;
                    comments.Add(comment);
                }
                else
                {
                    // Redireciona para o método AddReply se for um reply
                    return RedirectToAction("AddReply", new { parentCommentId = comment.ParentCommentId, replyText = comment.Text, replyUser = comment.User });
                }
                return RedirectToAction("Index");
            }
            return View(comment);
        }
    }
}