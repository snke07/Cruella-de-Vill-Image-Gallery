using CruellaDeVillImageGallery.Data;
using CruellaDeVillImageGallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CruellaDeVillImageGallery.Repositories
{
    public class CommentsRepository : BaseRepository
    {
        public static CommentModelFull PostComment(int userId, CommentModel model)
        {
            using (var context = new ImageLibraryEntities())
            {
                var comment = new Comment()
                {
                    AuthorId = userId,
                    Body = model.Body,
                    PictureId = model.PictureId
                };
                context.Comments.Add(comment);
                context.SaveChanges();

                return new CommentModelFull()
                {
                    Id = comment.Id,
                    AuthorId = userId,
                    Body = comment.Body,
                    PictureId = comment.PictureId
                };
            }
        }

        public static void DeleteComment(int commentId)
        {
            using (var context = new ImageLibraryEntities())
            {
                var comment = context.Comments.FirstOrDefault(c => c.Id == commentId);

                context.Comments.Remove(comment);
                context.SaveChanges();
            }
        }

        public static IEnumerable<Comment> GetAllComments(int pictureId)
        {
            using (var context = new ImageLibraryEntities())
            {
                var comments = context.Comments.Where(c => c.Id == pictureId).ToList();
                //var comments = new List<Comment>();

                //context.Comments
                //    .Where(c => c.Id == pictureId)
                //    .ToList()
                //    .ForEach(c => comments.Add(new Comment()
                //    {
                //        Id = c.Id,
                //        AuthorId = c.AuthorId,
                //        Body = c.Body,
                //        PictureId = c.PictureId
                //    }));
                return comments;
            }
        }
    }
}