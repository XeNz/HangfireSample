using HangfireSample.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HangfireSample.Api.Controllers
{
    [Route("api/commentreadhistories")]
    public class CommentReadHistoryController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentReadHistoryController(ICommentService commentService) => _commentService = commentService;

        /// <summary>
        /// GET api/commentreadhistories/{commentId}/count/
        /// </summary>
        /// <param name="commentId">The comment Id for which we are viewing the read history count</param>
        /// <remarks>Truthfully, this isn't how you're supposed to create REST endpoints</remarks>
        /// <returns></returns>
        [HttpGet("{commentId}/count")]
        public ActionResult<int> GetCount(int commentId)
        {
            return Ok(_commentService.GetHistoryCount(commentId));
        }
    }
}