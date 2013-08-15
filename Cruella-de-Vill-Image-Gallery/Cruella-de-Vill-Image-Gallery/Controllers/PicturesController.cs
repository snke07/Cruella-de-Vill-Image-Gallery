namespace CruellaDeVillImageGallery.Controllers
{
    using System.Net.Http;
    using System.Web.Http;
    using CruellaDeVillImageGallery.Models;
    using CruellaDeVillImageGallery.Repositories;
    using System.Collections.Generic;
    using System.Net;
    using System;

    public class PicturesController : BaseApiController
    {
        PicturesRepository repo = new PicturesRepository();

        [HttpPost]
        [ActionName("create")]
        public HttpResponseMessage CreateAlbum(string sessionKey, [FromBody] AlbumAddModel albumModel)
        {
            var response = this.PerformOperation(() =>
            {
                var userId = UsersRepository.LoginUser(sessionKey);
                var album = AlbumsRepository.CreateAlbum(userId, albumModel);

                return album;
            });

            return response;
        }

        [HttpDelete]
        [ActionName("delete")]
        public HttpResponseMessage DeleteAlbum(int albumId, string sessionKey)
        {
            var response = this.PerformOperation(() =>
            {
                var userId = UsersRepository.LoginUser(sessionKey);
                AlbumsRepository.DeleteAlbum(userId, albumId);
            });

            return response;
        }

        [HttpGet]
        [ActionName("load")]
        public HttpResponseMessage GetAlbum(int albumId, string sessionKey)
        {
            var response = this.PerformOperation(() =>
            {
                var userId = UsersRepository.LoginUser(sessionKey);
                var album = AlbumsRepository.GetAlbum(albumId);

                return album;
            });

            return response;
        }

        [HttpGet]
        [ActionName("getByAlbum")]
        public string GetAll(string sessionKey, int albumId)
        {
            try
            {
                var response = this.PerformOperation(() =>
                {
                    var userId = UsersRepository.LoginUser(sessionKey);
                    var pictures = new PicturesRepository().GetImagesByAlbumId(albumId);

                    return pictures;
                });
                return "it works";
                //return response;
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    return "it doesn't:" + e.InnerException.Message;
                }
                else
                {
                    return "it does not: " + e.Message;
                }
                //return this.Request.CreateResponse(HttpStatusCode.Forbidden, e.Message);
            }
        }

        [HttpGet]
        [ActionName("mine")]
        public HttpResponseMessage GetMine(string sessionKey)
        {
            var response = this.PerformOperation(() =>
            {
                var userId = UsersRepository.LoginUser(sessionKey);
                var albums = AlbumsRepository.GetMyAlbums(userId);

                return albums;
            });

            return response;
        }
    }
}

