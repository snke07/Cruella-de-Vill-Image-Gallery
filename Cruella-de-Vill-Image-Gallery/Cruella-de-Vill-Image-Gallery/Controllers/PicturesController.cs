using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using CruellaDeVillImageGallery.Repositories;
using CruellaDeVillImageGallery.Models;

namespace CruellaDeVillImageGallery.Controllers
{
    public class PicturesController : BaseApiController
    {
        PicturesRepository repo = new PicturesRepository();
        private const string LOCAL_TEST_ADDRESS = @"D:\currnet.jpg";
        private const string CLOUD_ADDRESS = @"current.jpg";

        [HttpPost]
        public async Task<HttpResponseMessage> UploadFiles()
        {
            string addressString = CLOUD_ADDRESS;
            //addressString = LOCAL_TEST_ADDRESS;

            if (!this.Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            MultipartMemoryStreamProvider provider = await this.Request.Content.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(new MultipartMemoryStreamProvider());

            foreach (var content in provider.Contents)
            {
                Stream st = content.ReadAsStreamAsync().Result;

                using (FileStream writer = File.Create(addressString))
                {
                    byte[] buffer = new byte[8 * 1024];
                    int len;

                    while ((len = st.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        writer.Write(buffer, 0, len);
                    }
                }
            }

            var title = HttpUtility.ParseQueryString(this.Request.RequestUri.Query).Get("title");
            var albumId = int.Parse(HttpUtility.ParseQueryString(this.Request.RequestUri.Query).Get("album_id"));

            var id = repo.AddImage(albumId, title, Guid.NewGuid().ToString() + ".jpg", addressString);
            
            var response = this.Request.CreateResponse(HttpStatusCode.Created, id);
            return response;
        }

        [HttpGet]
        [ActionName("getByAlbum")]
        public HttpResponseMessage GetByAlbumId(int albumId)
        {
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public HttpResponseMessage DeletePictureById(int id)
        {
            repo.RemoveImage(id);

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
