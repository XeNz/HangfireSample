using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HangfireSample.Business.Models;

namespace HangfireSample.Business.Services.Interfaces
{
    public interface ICommentService : IDisposable
    {
        Task<IEnumerable<Comment>> GetAllComments();
        Task<Comment> GetCommentById(int commentId);
        int GetHistoryCount(int commentId);
        Task UpdateHistoryCount();
    }
}