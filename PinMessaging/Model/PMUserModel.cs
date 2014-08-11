
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Newtonsoft.Json;
using PinMessaging.Other;
using PinMessaging.Utils;

namespace PinMessaging.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PMUserModel
    {
        [JsonProperty] [DefaultValue(null)] public string Id { get; set; }
        [JsonProperty] [DefaultValue(null)] public string CreatedTime { get; set; }
        [JsonProperty] [DefaultValue(null)] public string NbrMessage { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Points { get; set; }
        [JsonProperty] [DefaultValue(null)] public string NbrPin { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Email { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Pseudo { get; set; }
        [JsonProperty] [DefaultValue(null)] public string SimId { get; set; }
        [JsonProperty] [DefaultValue(null)] public PMGradeModel Grade { get; set; }
                       [DefaultValue(null)] public BitmapImage ProfilPicture { get; set; }

        public PMUserModel Clone()
        {
            var model = new PMUserModel
            {
                Id = Id,
                CreatedTime = CreatedTime,
                NbrMessage = NbrMessage,
                Points = Points,
                NbrPin = NbrPin,
                Email = Email,
                Pseudo = Pseudo,
                SimId = SimId,
                Grade = Grade
            };
            return model;
        }

        public PMUserModel()
        {
            if (ProfilPicture == null)
                if (Design.ProfilPictureUpdateUi(ProfilPicture, Id) == null)
                    ProfilPicture = PMData.NeutralProfilPic;
        }

        public void ShowUserContent()
        {
            try
            {
                Logs.Output.ShowOutput((Id == null ? "" : "id:" + Id) + 
                                        (NbrMessage == null ? "" : " nbrMessage:" + NbrMessage) + 
                                        (Points == null ? "" : " points:" + Points) +
                                        (NbrPin == null ? "" : " nbrpin:" + NbrPin) +
                                        (Email == null ? "" : " email:" + Email) +
                                        (Pseudo == null ? "" : " login:" + Pseudo) +
                                        (CreatedTime == null ? "" : " createdtime:" + CreatedTime) +
                                        (Grade == null ? "" : " grade:" + Grade));
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("User content is incorrectly formatted", Logs.Error.ErrorsPriority.NotCritical);
            }
            Grade.ShowGradeContent();
        }
    }
}
