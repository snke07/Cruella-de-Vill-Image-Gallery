namespace CruellaDeVillImageGallery.Controllers
{
    using System.Net.Http;
    using System.Web.Http;
    using CruellaDeVillImageGallery.Models;
    using CruellaDeVillImageGallery.Repositories;
    using System.Collections.Generic;

    public class CommentsController : BaseApiController
    {
        [HttpPost]
        [ActionName("post")]
        public HttpResponseMessage PostComment(string sessionKey, [FromBody] CommentModel commentModel)
        {
            var response = this.PerformOperation(() =>
            {
                var userId = UsersRepository.LoginUser(sessionKey);
                var comment = CommentsRepository.PostComment(userId, commentModel);
                return comment;
            });
            return response;
        }

        [HttpDelete]
        [ActionName("delete")]
        public HttpResponseMessage DeleteComment(string sessionKey, int commentId)
        {
            var response = this.PerformOperation(() =>
            {
                //var userId = UsersRepository.LoginUser(sessionKey);
                CommentsRepository.DeleteComment(commentId);
            });

            return response;
        }

        [HttpGet]
        [ActionName("all")]
        public HttpResponseMessage GetAllComments(string sessionKey, int pictureId)
        {
            var response = this.PerformOperation(() =>
            {
                var comments = CommentsRepository.GetAllComments(pictureId);
                var commentModels = new List<CommentModel>();
                foreach (var com in comments)
                {
                    commentModels.Add(new CommentModel()
                    {
                        Body = com.Body,
                        PictureId = com.PictureId
                    });
                }
                return commentModels;
            });
            return response;
        }
    }
}
