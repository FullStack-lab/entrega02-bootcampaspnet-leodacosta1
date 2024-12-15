using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using leodacosta02.Models.CommentModel;
using System.Web.Mvc;
using System.Xml.Linq;

namespace leodacosta02.Controllers.CommentController
{
    public class CommentController : Controller
    {
        // Lista com informação dos comentários postados, que simula um banco de dados
        private static List<CommentModel> comments = new List<CommentModel>
            {
                new CommentModel
                {
                    CommentId = 1,
                    User = "John",
                    Text = "This is a comment.",
                    CreatedAt = new DateTime(2024, 12, 11, 15, 23, 0),
                    RepliesCount = 2,
                    ParentCommentId = 0,
                    Replies = new List<CommentModel>
                    {
                        new CommentModel
                        {
                            CommentId = 101,
                            User = "Johnson",
                            Text = "This is a reply.",
                            CreatedAt = DateTime.Now,
                            ParentCommentId = 1,
                            RepliesCount = 0
                        },
                        new CommentModel
                        {
                            CommentId = 102,
                            User = "Jackson",
                            Text = "This is a 2nd reply.",
                            CreatedAt = DateTime.Now,
                            ParentCommentId = 1,
                            RepliesCount = 0
                        }
                    }
                }

            };
        // Página principal que exibe os comentários em formato de lista
        public ActionResult Index()
        {
            return View(comments);
        }
        // Página pra criação de novo comentário
        [HttpGet]
        public ActionResult Create(int? parentCommentId = null)
        {
            var comment = new CommentModel
            {
                ParentCommentId = parentCommentId ?? 0
            };
            return View(comment);
        }
        // método para criação de comentários e replies
        [HttpPost]
        public ActionResult Create(CommentModel comment)
        {
            if (ModelState.IsValid)
            {
                // Procura se há itens na lista
                if (comments.Count == 0)
                {
                    comment.CommentId = 1; // se não, o item ganha valor de 1
                }
                else
                {
                    comment.CommentId = comments.Max(t => t.CommentId) + 1; // se sim, o valor é adicionado 1 ao maior valor de id
                }
                comment.CreatedAt = DateTime.Now;

                // se o ParentCommentId é maior que 0, significa que há um comentário pai, e portanto este é um reply
                if (comment.ParentCommentId > 0)
                {
                    // procura pelos comentários e encontra o pai relacionado
                    var parentComment = comments.FirstOrDefault(c => c.CommentId == comment.ParentCommentId);

                    // se não achou nos comentários pai, procura nos replies
                    if (parentComment == null)
                    {
                        parentComment = comments
                            .SelectMany(c => c.Replies)
                            .FirstOrDefault(r => r.CommentId == comment.ParentCommentId);
                    }

                    else
                    {
                        // cria a lista de replies, caso ela não exista
                        if (parentComment.Replies == null)
                        {
                            parentComment.Replies = new List<CommentModel>();
                        }

                        // adiciona um reply à lista de replies do comentário pai
                        parentComment.Replies.Add(comment);
                        parentComment.RepliesCount++;
                    }
                }
                else
                {
                    // se é um comentário pai:
                    comments.Add(comment);
                }

                return RedirectToAction("Index");
            }
            return View(comment);
        }
    }
}