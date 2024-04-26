using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> CommentExists(int id)
        {
            var comment = await _context.Comment.FindAsync(id);
            return comment != null;
        }

        public async Task<Comment> CreateAsync(Comment entity)
        {
            await _context.Comment.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel = await _context.Comment.FirstOrDefaultAsync(x => x.Id == id);
            if(commentModel == null)
            {
                return null;
            }
            _context.Comment.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<List<Comment>> GetAllasync()
        {
            return await _context.Comment.Include(c => c.AppUser).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            var comment = await _context.Comment.Include(c => c.AppUser).FirstOrDefaultAsync(c => c.Id == id);
            if(comment == null)
            {
                return null;
            }
            return comment;
        }

        public async Task<Comment?> UpdateAsync(int id, Comment entity)
        {
            var exisitngComment = await this.GetByIdAsync(id);
            if(exisitngComment == null)
            {
                return null;
            }

            exisitngComment.Title = entity.Title;
            exisitngComment.Content = entity.Content;
            await _context.SaveChangesAsync();
            return exisitngComment;
        }
    }
}
