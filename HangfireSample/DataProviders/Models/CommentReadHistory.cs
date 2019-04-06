using System;

namespace HangfireSample.DataProviders.Models
{
    public class CommentReadHistory
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public DateTime ReadDate { get; set; }

        public CommentReadHistory(int commentId, DateTime readDate)
        {
            CommentId = commentId;
            ReadDate = readDate;
        }
    }
}