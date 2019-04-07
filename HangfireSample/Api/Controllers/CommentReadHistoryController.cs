using System.Threading.Tasks;
using HangfireSample.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HangfireSample.Api.Controllers
{
    [Route("api/commentreadhistories")]
    public class CommentReadHistoryController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentReadHistoryController(ICommentService commentService) => _commentService = commentService;

        // GET api/values
        [HttpGet("{commentId}/count")]
        public ActionResult<int> GetCount(int commentId)
        {
            return Ok(_commentService.GetHistoryCount(commentId));
        }
    }
}