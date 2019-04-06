using System.Collections.Generic;
using System.Threading.Tasks;
using HangfireSample.Business.Models;

namespace HangfireSample.Business.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllComments();
        Task<Comment> GetCommentById();
    }
}