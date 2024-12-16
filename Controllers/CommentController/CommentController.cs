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
                    Text = "Este é um comentário.",
                    CreatedAt = new DateTime(2024, 12, 11, 15, 23, 0),
                    RepliesCount = 2,
                    ParentCommentId = 0,
                    Replies = new List<CommentModel>
                    {
                        new CommentModel
                        {
                            CommentId = 101,
                            User = "Johnson",
                            Text = "Esta é uma resposta.",
                            CreatedAt = DateTime.Now,
                            ParentCommentId = 1,
                            RepliesCount = 0
                        },
                        new CommentModel
                        {
                            CommentId = 102,
                            User = "Jackson",
                            Text = "Esta é uma segunda resposta.",
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
        
        // Método GET para exibir página de criação de novo comentário / reply
        [HttpGet]
        public ActionResult Create(int? parentCommentId = null)
        {
            var comment = new CommentModel
            {
                ParentCommentId = parentCommentId ?? 0
            };
            return View(comment);
        }
        // Método POST para criação de comentários e replies
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
        
        // Helper method pra procurar pelos comentários e replies
        private static CommentModel FindCommentById(int commentId, List<CommentModel> commentList)
        {
            foreach (var comment in commentList)
            {
                if (comment.CommentId == commentId)
                {
                    return comment;
                }

                if (comment.Replies != null && comment.Replies.Count > 0)
                {
                    var foundReply = FindCommentById(commentId, comment.Replies);
                    if (foundReply != null)
                    {
                        return foundReply;
                    }
                }
            }

            return null;
        }
        // Método GET -atualizado com FindCommentById- para exibir página de edição de comentários / replies
        [HttpGet]
        public ActionResult Edit(int CommentId)
        {
            var comment = FindCommentById(CommentId, comments);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }
        //Método POST para editar comentário ou reply
        [HttpPost]
        public ActionResult Edit(CommentModel updatedComment)
        {
           if (ModelState.IsValid)
            {
                
                var comment = FindCommentById(updatedComment.CommentId, comments);

                if (comment == null)
                {
                    return HttpNotFound();
                }

                
                comment.Text = updatedComment.Text;

                return RedirectToAction("Index"); 
            }

            return View(updatedComment); 
        }

        // Método GET que abre página de confirmação antes de excluir comentário ou reply
        [HttpGet]
        public ActionResult Delete(int CommentId)
        {
            var comment = FindCommentById(CommentId, comments);

            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }
        // Método POST que exclui comentário ou reply
        [HttpPost]
        public ActionResult DeleteConfirmed(int CommentId)
        {
            var comment = FindCommentById(CommentId, comments);

            if (comment == null)
            {
                return HttpNotFound();
            }

            // Se for um reply, ele é removido da lista de Replies pai
            if (comment.ParentCommentId != 0)
            {
                var parent = FindCommentById((int)comment.ParentCommentId, comments);
                parent?.Replies.Remove(comment);
            }
            else
            {
                // Se for um comentário que não é reply, remove ele da lista principal
                comments.Remove(comment);
            }

            return RedirectToAction("Index");
        }

        // Método GET pra buscar detalhes de um comentário
        [HttpGet]
        public ActionResult Details(int CommentId)
        {
            var comment = FindCommentById(CommentId, comments);

            if (comment == null)
            {
                return HttpNotFound();
            }

            return View(comment);
        }
    }
}