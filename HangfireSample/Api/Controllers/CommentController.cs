using System.Collections.Generic;
using System.Threading.Tasks;
using HangfireSample.Business.Models;
using HangfireSample.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HangfireSample.Api.Controllers
{
    [Route("api/comments")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService) => _commentService = commentService;

        /// <summary>
        /// Get all comments
        /// GET api/comments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> Get()
        {
            return Ok(await _commentService.GetAllComments());
        }

        /// <summary>
        /// Get a comment by Id
        /// 
        /// GET api/comments/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> Get(int id)
        {
            return Ok(await _commentService.GetCommentById(id));
        }
    }
}