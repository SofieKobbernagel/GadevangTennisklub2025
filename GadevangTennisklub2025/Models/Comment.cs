namespace GadevangTennisklub2025.Models
{
    public class Comment
    {
        public int Id { get; set; } 
        public int BlogId { get; set; } 
        public int MemberId { get; set; } 
        public string CommentContent { get; set; }

        public Comment() { }
        public Comment(int id, int blogId, int memberId, string Content)
        {
            Id = id;
            BlogId = blogId;
            MemberId = memberId;
            CommentContent = Content;
        }

    }
}
