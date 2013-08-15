using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CruellaDeVillImageGallery.Data;
using CruellaDeVillImageGallery.Models;
using Dropbox;

namespace CruellaDeVillImageGallery.Repositories
{
    public class PicturesRepository : BaseRepository
    {
        private ImageLibraryEntities context;
        private DropboxClient client;

        public PicturesRepository()
        {
            this.context = new ImageLibraryEntities();
            //this.client = new DropboxClient();
        }

        public int AddImage(int albumnId, string title, string fileName, string filePath)
        {
            this.client.UploadFile(filePath, fileName);
            Picture pic = new Picture();
            pic.AlbumId = albumnId;
            pic.Title = title;
            pic.BaseUrl = this.client.UploadFile(filePath, fileName);
            pic.ThumbUrl = pic.BaseUrl;
            context.Pictures.Add(pic);

            context.SaveChanges();

            return pic.Id;
        }

        public void RemoveImage(int id)
        {
            var picEntity = (from picture in context.Pictures
                             where picture.Id == id
                             select picture).FirstOrDefault();

            context.Pictures.Remove(picEntity);
            context.SaveChanges();
        }
    }
}