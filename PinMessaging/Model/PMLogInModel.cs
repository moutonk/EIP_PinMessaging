
using System.ComponentModel;
using System.Runtime.Serialization;
namespace PinMessaging.Model
{
    [DataContract]
    public class PMLogInModel
    {
        [DefaultValue("")] [DataMember] public string Email;

        [DefaultValue("")] [DataMember] public string Password;

        [DefaultValue("")] [DataMember] public string PasswordRetyped;

        [DefaultValue("")] [DataMember] public string PhoneSimId;
    }
}

