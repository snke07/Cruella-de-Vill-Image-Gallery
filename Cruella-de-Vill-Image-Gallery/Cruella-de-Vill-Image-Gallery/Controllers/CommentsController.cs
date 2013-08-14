namespace CruellaDeVillImageGallery.Controllers
{
    using System.Net.Http;
    using System.Web.Http;
    using CruellaDeVillImageGallery.Models;
    using CruellaDeVillImageGallery.Repositories;

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
                var userId = UsersRepository.LoginUser(sessionKey);
                var comments = CommentsRepository.GetAllComments(userId);
                return comments;
            });
            return response;
        }
    }
}
