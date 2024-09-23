using Finshark.Data;
using Finshark.Helpers;
using Finshark.Interfaces;
using Finshark.Models;
using Microsoft.EntityFrameworkCore;

namespace Finshark.Repository
{
    public class EFCommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public EFCommentRepository(ApplicationDBContext applicationDBContext)
        {
            _dbContext = applicationDBContext;
        }

        public async Task<List<Comment>> GetAllAsync(CommentQueryObject queryObject)
        {
            var comments =  _dbContext.Comments.Include(a => a.AppUser).AsQueryable();
                
             if(!string.IsNullOrEmpty(queryObject.Symbol))
            {
                comments = comments.Where( s => s.Stock.Symbol == queryObject.Symbol );
            }

             if(queryObject.IsDescending == true)
            {
                comments = comments.OrderByDescending( c => c.CreatedOn );
            }

             return await comments.ToListAsync();


        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _dbContext.Comments.Include(a => a.AppUser).FirstOrDefaultAsync( c => c.Id == id);
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _dbContext.Comments.AddAsync(commentModel);
            await _dbContext.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var existingComment = await _dbContext.Comments.FindAsync(id);
            if (existingComment == null)
            {
                return null;
            }
            existingComment.Title = commentModel.Title;
            existingComment.Content = commentModel.Content;
            await _dbContext.SaveChangesAsync();
            return existingComment;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel = await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if(commentModel == null)
            {
                return null;
            }
            _dbContext.Comments.Remove(commentModel);
            await _dbContext.SaveChangesAsync();
            return commentModel;
        }
    }
}
