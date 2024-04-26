using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllasync();
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment entity);
        Task<bool> CommentExists(int id);
        Task<Comment?> UpdateAsync(int id, Comment entity);
        Task<Comment?> DeleteAsync(int id);
    }
}
