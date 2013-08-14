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
                var comment = CommentsRepository.PostComment(commentModel.AuthorId, commentModel.Body, commentModel.PictureId);
                return comment;
            });
            return response;
        }

        [HttpGet]
        [ActionName("unread")]
        public HttpResponseMessage GetUnreadComments(string sessionKey)
        {
            var response = this.PerformOperation(() =>
            {
                var userId = UsersRepository.LoginUser(sessionKey);
                var comments = CommentsRepository.GetUnreadComments(userId);
                return comments;
            });
            return response;
        }

        [HttpGet]
        [ActionName("all")]
        public HttpResponseMessage GetAllMessages(string sessionKey)
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
