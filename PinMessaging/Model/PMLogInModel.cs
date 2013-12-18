
using System.Runtime.Serialization;
namespace PinMessaging.Model
{
    [DataContract]
    public class PMLogInModel
    {
        [DataMember]
        public string Email  {get; set;}

        [DataMember]
        public string Password = "";

        [DataMember]
        public string PasswordRetyped = "";

        [DataMember]
        public string PhoneSimId = "";
    }
}

