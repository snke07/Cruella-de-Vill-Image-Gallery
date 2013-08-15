namespace CruellaDeVillImageGallery.Controllers
{
    using System.Net.Http;
    using System.Web.Http;
    using CruellaDeVillImageGallery.Models;
    using CruellaDeVillImageGallery.Repositories;
    using System.Net;

    public class PicturesController : BaseApiController
    {
        [HttpGet]
        [ActionName("all")]
        public HttpResponseMessage GetAll(int albumId)
        {
            var response = this.PerformOperation(() =>
            {
                var repo = new PicturesRepository();
                var pictures = repo.GetImagesByAlbumId(albumId);

                return pictures;
            });

            return response;
        }
    }
}
