using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                Title = comment.Title,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
                CreatedBy = comment.AppUser.UserName,
                StockId = comment.StockId
            };
        }

        public static Comment ToComment(this CreateCommentDto createCommentDto, int stockId)
        {
            return new Comment
            {
                StockId = stockId,
                Title = createCommentDto.Title,
                Content = createCommentDto.Content
            };
        }

        public static Comment toComment(this UpdateCommentDto updateCommentDto) 
        {
            return new Comment
            {
                Content = updateCommentDto.Content,
                Title = updateCommentDto.Title,
            };
        }
    }
}
