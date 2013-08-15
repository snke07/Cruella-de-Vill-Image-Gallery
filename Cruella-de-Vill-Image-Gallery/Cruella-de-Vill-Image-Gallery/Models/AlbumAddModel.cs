namespace CruellaDeVillImageGallery.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class AlbumAddModel
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "parentId")]
        public int? ParentId { get; set; }
    }

    [DataContract]
    public class AlbumOverviewModel : AlbumAddModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "ownerId")]
        public int OwnerId { get; set; }
    }

    public class AlbumFullModel : AlbumOverviewModel
    {
        [DataMember(Name = "subalbums")]
        public ICollection<AlbumOverviewModel> SubAlbums { get; set; }

        [DataMember(Name = "pictures")]
        public ICollection<PictureModel> Pictures { get; set; }
    }
}