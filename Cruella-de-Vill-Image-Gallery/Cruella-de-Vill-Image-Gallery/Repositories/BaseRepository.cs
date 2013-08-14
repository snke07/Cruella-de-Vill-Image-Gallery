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
    }
}