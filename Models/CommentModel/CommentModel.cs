using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace leodacosta02.Models.CommentModel
{
    public class CommentModel
    {
        public int CommentId { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CommentModel> Replies { get; set; } = new List<CommentModel>();
        public int RepliesCount { get; set; }
        public int? ParentCommentId { get; set; }
    }
}