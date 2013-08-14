namespace CruellaDeVillImageGallery.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class AlbumModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

    }
}