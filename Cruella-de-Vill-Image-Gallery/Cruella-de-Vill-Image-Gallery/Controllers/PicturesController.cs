namespace CruellaDeVillImageGallery.Controllers
{
    using System.Net.Http;
    using System.Web.Http;
    using CruellaDeVillImageGallery.Models;
    using CruellaDeVillImageGallery.Repositories;
    using System.Collections.Generic;
    using System.Net;
    using System;
    using System.IO;
    using System.Web;

    public class PicturesController : BaseApiController
    {
        PicturesRepository repo = new PicturesRepository();

        [HttpDelete]
        [ActionName("delete")]
        public HttpResponseMessage DeleteAlbum(string sessionKey, int id)
        {
            var response = this.PerformOperation(() =>
            {
                var userId = UsersRepository.LoginUser(sessionKey);
                repo.RemoveImage(id);
            });

            return response;
        }

        [HttpPost]
        [ActionName("add")]
        public HttpResponseMessage AddAlbum(string sessionKey, int albumId, string title)
        {
            string addressString = @"current.jpg";
            //addressString = LOCAL_TEST_ADDRESS;

            if (!this.Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            MultipartMemoryStreamProvider provider = Request.Content.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(new MultipartMemoryStreamProvider()).Result;

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

            var id = repo.AddImage(albumId, title, Guid.NewGuid().ToString() + ".jpg", addressString);
            
            var response = this.Request.CreateResponse(HttpStatusCode.Created, id);
            return response;
        }
    }
}

