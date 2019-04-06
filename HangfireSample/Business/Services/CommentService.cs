using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Hangfire;
using HangfireSample.Business.Models;
using HangfireSample.Business.Services.Interfaces;
using HangfireSample.DataProviders;
using HangfireSample.DataProviders.Models;
using Newtonsoft.Json;

namespace HangfireSample.Business.Services
{
    public class CommentService : ICommentService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly EntityContext _entityContext;
        private const string CommentApi = "CommentApi";

        public CommentService(IHttpClientFactory httpClientFactory, EntityContext entityContext)
        {
            _httpClientFactory = httpClientFactory;
            _entityContext = entityContext;
        }

        public async Task<IEnumerable<Comment>> GetAllComments()
        {
            var client = _httpClientFactory.CreateClient(CommentApi);
            var responseMessage = await client.GetAsync("comments");

            return JsonConvert.DeserializeObject<IEnumerable<Comment>>(await responseMessage.Content.ReadAsStringAsync());
        }

        public async Task<Comment> GetCommentById(int commentId)
        {
            var client = _httpClientFactory.CreateClient(CommentApi);
            var responseMessage = await client.GetAsync($"comments/{commentId}");

            var deserializedObject =  JsonConvert.DeserializeObject<Comment>(await responseMessage.Content.ReadAsStringAsync());

            BackgroundJob.Enqueue(() => UpdateCommentReadCounterJob(commentId, DateTime.UtcNow));
            return deserializedObject;
        }

        public async Task UpdateCommentReadCounterJob(int commentId, DateTime readDate)
        {
            await _entityContext.CommentReadHistories.AddAsync(new CommentReadHistory(commentId, readDate));
            await _entityContext.SaveChangesAsync();
        }
    }
}