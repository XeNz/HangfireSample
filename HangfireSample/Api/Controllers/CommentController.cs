using System.Collections.Generic;
using System.Threading.Tasks;
using HangfireSample.Business.Models;
using HangfireSample.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HangfireSample.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService) => _commentService = commentService;

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> Get() => Ok(await _commentService.GetAllComments());

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> Get(int id) => Ok(await _commentService.GetCommentById(id));
    }
}