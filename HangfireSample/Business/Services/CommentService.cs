using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hangfire;
using HangfireSample.Business.Models;
using HangfireSample.Business.Services.Interfaces;
using HangfireSample.DataProviders;
using HangfireSample.DataProviders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace HangfireSample.Business.Services
{
    public class CommentService : ICommentService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly EntityContext _entityContext;
        private readonly IMemoryCache _memoryCache;
        private const string CommentApi = "CommentApi";
        private const string HistoryCacheKey = "CommentHistoryEntry_";

        public CommentService(IHttpClientFactory httpClientFactory, EntityContext entityContext,
            IMemoryCache memoryCache)
        {
            _httpClientFactory = httpClientFactory;
            _entityContext = entityContext;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<Comment>> GetAllComments()
        {
            var client = _httpClientFactory.CreateClient(CommentApi);
            var responseMessage = await client.GetAsync("comments");

            return JsonConvert.DeserializeObject<IEnumerable<Comment>>(
                await responseMessage.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// This method has 2 functional purposes:
        /// 1. Get a comment by commentId from an external service
        /// 2. After receiving this comment, launch a fire-and-forget job that creates a 'CommentReadHistory' entry.
        /// </summary>
        /// <param name="commentId">The comment Identifier</param>
        /// <returns></returns>
        public async Task<Comment> GetCommentById(int commentId)
        {
            var client = _httpClientFactory.CreateClient(CommentApi);
            var responseMessage = await client.GetAsync($"comments/{commentId}");

            var deserializedObject =
                JsonConvert.DeserializeObject<Comment>(await responseMessage.Content.ReadAsStringAsync());

            BackgroundJob.Enqueue(() => UpdateCommentReadCounterJob(commentId, DateTime.UtcNow));
            return deserializedObject;
        }

        public int GetHistoryCount(int commentId)
        {
            _memoryCache.TryGetValue($"{HistoryCacheKey}{commentId}_count", out int? item);
            return item ?? 0;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public async Task UpdateCommentReadCounterJob(int commentId, DateTime readDate)
        {
            await _entityContext.CommentReadHistories.AddAsync(new CommentReadHistory(commentId, readDate));
            await _entityContext.SaveChangesAsync();
        }

        public async Task UpdateHistoryCount()
        {
            var commentReadHistories = await _entityContext.CommentReadHistories.ToListAsync();
            if (commentReadHistories == null) return;

            var distinctIds = commentReadHistories.Select(x => x.CommentId).Distinct();
            Parallel.ForEach(distinctIds, (commentId) =>
            {
                var countForId = commentReadHistories.Count(x => x.CommentId == commentId);
                _memoryCache.TryGetValue($"{HistoryCacheKey}{commentId}_count", out int? originalCount);
                if (!originalCount.HasValue)
                    _memoryCache.Set($"{HistoryCacheKey}{commentId}_count", countForId);
                if (originalCount.HasValue && originalCount.Value != countForId)
                {
                    _memoryCache.Remove($"{HistoryCacheKey}{commentId}_count");
                    _memoryCache.Set($"{HistoryCacheKey}{commentId}_count", countForId);
                }
            });
        }

        public void Dispose()
        {
        }
    }
}