﻿namespace CruellaDeVillImageGallery.Controllers
{
    using System.Net.Http;
    using System.Web.Http;
    using CruellaDeVillImageGallery.Models;
    using CruellaDeVillImageGallery.Repositories;

    public class AlbumsController : BaseApiController
    {
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
        [ActionName("all")]
        public HttpResponseMessage GetAll(string sessionKey)
        {
            var response = this.PerformOperation(() =>
            {
                var userId = UsersRepository.LoginUser(sessionKey);
                var albums = AlbumsRepository.GetAllAlbums();

                return albums;
            });

            return response;
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
