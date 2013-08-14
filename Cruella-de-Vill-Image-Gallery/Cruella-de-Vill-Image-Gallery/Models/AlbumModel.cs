namespace CruellaDeVillImageGallery.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class AlbumModel
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "parentId")]
        public int? ParentId { get; set; }
    }

    [DataContract]
    public class AlbumGetModel : AlbumModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "ownerId")]
        public int OwnerId { get; set; }
    }
}