namespace CruellaDeVillImageGallery.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class CommentModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "authorId")]
        public int AuthorId { get; set; }

        [DataMember(Name = "pictureId")]
        public int PictureId { get; set; }

        [DataMember(Name = "body")]
        public string Body { get; set; }

    }
}