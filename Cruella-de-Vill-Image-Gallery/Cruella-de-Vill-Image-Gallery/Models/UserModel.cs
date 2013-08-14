namespace CruellaDeVillImageGallery.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class UserLoginModel
    {
        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "authCode")]
        public string AuthCode { get; set; }
    }

    [DataContract]
    public class UserRegisterModel : UserLoginModel
    {
        [DataMember(Name = "nickname")]
        public string Nickname { get; set; }
    }

    [DataContract]
    public class UserLoggedModel
    {
        [DataMember(Name = "sessionKey")]
        public string SessionKey { get; set; }

        [DataMember(Name = "nickname")]
        public string Nickname { get; set; }
    }
}