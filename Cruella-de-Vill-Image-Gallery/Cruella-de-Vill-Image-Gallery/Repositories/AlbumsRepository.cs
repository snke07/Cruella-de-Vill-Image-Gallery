namespace CruellaDeVillImageGallery.Repositories
{
    using System.Collections.Generic;
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

        public static AlbumOverviewModel CreateAlbum(int userId, AlbumAddModel albumModel)
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

                return new AlbumOverviewModel()
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

        public static AlbumFullModel GetAlbum(int albumId)
        {
            using (var context = new ImageLibraryEntities())
            {
                var album = GetAlbum(albumId, context);
                var subalbums = new List<AlbumOverviewModel>();
                var pictures = new List<PictureOverviewModel>();

                context.Albums
                    .Where(a => a.ParentId == album.Id)
                    .ToList()
                    .ForEach(a => subalbums.Add(new AlbumOverviewModel()
                        {
                            Id = a.Id,
                            OwnerId = a.UserId,
                            ParentId = a.ParentId,
                            Title = a.Title
                        }));

                context.Pictures
                    .Where(p => p.AlbumId == albumId)
                    .ToList()
                    .ForEach(p => pictures.Add(new PictureOverviewModel()
                        {
                            Id = p.Id,
                            Title = p.Title,
                            ThumbUrl = p.ThumbUrl
                        }));

                return new AlbumFullModel()
                {
                    Id = album.Id,
                    OwnerId = album.UserId,
                    ParentId = album.ParentId,
                    Title = album.Title,
                    SubAlbums = subalbums,
                    Pictures = pictures
                };
            }
        }

        public static IEnumerable<AlbumOverviewModel> GetAllAlbums()
        {
            using (var context = new ImageLibraryEntities())
            {
                var albumModels = new List<AlbumOverviewModel>();

                context.Albums
                    .Where(a => a.ParentId == null)
                    .ToList()
                    .ForEach(a => albumModels.Add(new AlbumOverviewModel()
                    {
                        Id = a.Id,
                        OwnerId = a.UserId,
                        ParentId = a.ParentId,
                        Title = a.Title
                    }));

                return albumModels;
            }
        }

        public static IEnumerable<AlbumOverviewModel> GetMyAlbums(int userId)
        {
            using (var context = new ImageLibraryEntities())
            {
                var albumModels = new List<AlbumOverviewModel>();

                context.Albums
                    .Where(a => a.UserId == userId)
                    .Where(a => a.ParentId == null)
                    .ToList()
                    .ForEach(a => albumModels.Add(new AlbumOverviewModel()
                        {
                            Id = a.Id,
                            OwnerId = a.UserId,
                            ParentId = a.ParentId,
                            Title = a.Title
                        }));

                return albumModels;
            }
        }
    }
}