namespace HangfireSample.Business.Models
{
    public class Comment
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
    }
}