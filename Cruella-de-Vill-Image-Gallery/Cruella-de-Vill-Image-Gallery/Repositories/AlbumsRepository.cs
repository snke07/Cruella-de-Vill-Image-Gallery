namespace CruellaDeVillImageGallery.Repositories
{
    using System.Linq;
    using CruellaDeVillImageGallery.Data;
    using CruellaDeVillImageGallery.Models;

    public class AlbumsRepository : BaseRepository
    {
        private const string ValidAlbumChars = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM_1234567890- ";
        private const int MinAlbumChars = 6;
        private const int MaxAlbumChars = 30;

        private static void ValidateAlbumTitle(string albumTitle)
        {
            if (albumTitle == null || albumTitle.Length < MinAlbumChars || albumTitle.Length > MaxAlbumChars)
            {
                throw new ServerErrorException("Email should be between 6 and 30 symbols long", "INV_ALBM_LEN");
            }
            else if (albumTitle.Any(ch => !ValidAlbumChars.Contains(ch)))
            {
                throw new ServerErrorException("Email contains invalid characters", "INV_ALBM_CHARS");
            }
        }

        private static void ValidateAlbumParent(Album album, int userId, ImageLibraryEntities context)
        {
            if (album.ParentId == null)
            {
                return;
            }

            var parentAlbum = GetAlbum((int)album.ParentId, context);

            if (parentAlbum.UserId != userId)
            {
                throw new ServerErrorException("Unauthorized access", "ERR_INV_OWNR");
            }
        }

        public static AlbumGetModel CreateAlbum(int userId, AlbumModel albumModel)
        {
            ValidateAlbumTitle(albumModel.Title);

            using (var context = new ImageLibraryEntities())
            {
                var album = new Album()
                {
                    UserId = userId,
                    Title = albumModel.Title,
                    ParentId = albumModel.ParentId
                };

                ValidateAlbumParent(album, userId, context);

                context.Albums.Add(album);
                context.SaveChanges();

                return new AlbumGetModel()
                {
                    Id = album.Id,
                    OwnerId = album.UserId,
                    ParentId = album.ParentId,
                    Title = album.Title
                };
            }
        }

        public static void DeleteAlbum(int userId, int albumId)
        {
            using (var context = new ImageLibraryEntities())
            {
                var album = GetOwnAlbum(albumId, userId, context);

                context.Albums.Remove(album);
                context.SaveChanges();
            }
        }

        public static AlbumGetModel GetAlbum(int albumId)
        {
            using (var context = new ImageLibraryEntities())
            {
                var album = GetAlbum(albumId, context);

                return new AlbumGetModel()
                {
                    Id = album.Id,
                    OwnerId = album.UserId,
                    ParentId = album.ParentId,
                    Title = album.Title
                };
            }
        }
    }
}