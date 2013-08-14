namespace CruellaDeVillImageGallery.Repositories
{
    using System;
    using System.Linq;
    using CruellaDeVillImageGallery.Data;
    using CruellaDeVillImageGallery.Models;

    public class BaseRepository
    {
        protected const int Sha1CodeLength = 40;
        protected static Random rand = new Random();

        protected static User GetUser(int userId, ImageLibraryEntities context)
        {
            var user = context.Users
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new ServerErrorException("Invalid user", "ERR_INV_USR");
            }

            return user;
        }

        protected static Album GetAlbum(int albumId, ImageLibraryEntities context)
        {
            var album = context.Albums
                .FirstOrDefault(a => a.Id == albumId);

            if (album == null)
            {
                throw new ServerErrorException("Invalid album", "ERR_INV_ALBM");
            }

            return album;
        }

        protected static Album GetOwnAlbum(int albumId, int userId, ImageLibraryEntities context)
        {
            var album = GetAlbum(albumId, context);

            if (album.UserId != userId)
            {
                throw new ServerErrorException("Unauthorized access", "ERR_INV_OWNR");
            }

            return album;
        }
    }
}