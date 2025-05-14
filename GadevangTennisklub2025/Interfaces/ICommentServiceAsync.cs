using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Interfaces
{
    public interface ICommentServiceAsync
    {
        public Task CreateComment(Comment Co);
        public Task DeleteComment(Comment Co);
        public Task UpdateComment(Comment Co);
        public Task<Comment> GetComment(int id);
        public Task<List<Comment>> GetCommentsForPost(int Blogid);
        public Task<List<Comment>> GetAllComments();
    }
}
