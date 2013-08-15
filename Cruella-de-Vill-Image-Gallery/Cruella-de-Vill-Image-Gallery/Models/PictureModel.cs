namespace CruellaDeVillImageGallery.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class PictureModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "thumbUrl")]
        public string ThumbUrl { get; set; }

        [DataMember(Name = "baseUrl")]
        public string PictureUrl { get; set; }
    }
}