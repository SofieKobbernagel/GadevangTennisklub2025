using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Interfaces
{
    public interface IBlogPostServicesAsync
    {
        public Task CreateBlogPost(BlogPost bp);
        public Task DeleteBlogPost(BlogPost bp);
        public Task UpdateBlogPost(BlogPost bp);
        public Task<BlogPost> GetBlogPost(int id);
        public Task<List<BlogPost>> GetAllBlogPost();
    }
}
