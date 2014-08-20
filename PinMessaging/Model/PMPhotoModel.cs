using System;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using PinMessaging.Utils;

namespace PinMessaging.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PMPhotoModel
    {
        [JsonProperty] public string UserId { get; set; }
        [JsonProperty] public byte[] FieldBytes { get; set; }
        public BitmapImage Img { get; set; }

        public PMPhotoModel()
        {
            Img = null;
            UserId = "-1";
        }

        public void CreateStream()
        {
            try
            {
                if (Img == null)
                {
                    Img = new BitmapImage();
                    Img.SetSource(new MemoryStream(FieldBytes));
                }
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("CreateStream: " + exp.StackTrace + Environment.NewLine, exp, Logs.Error.ErrorsPriority.NotCritical);
                Img = null;
            }
        }

        [OnDeserialized]
        private void CreateStream(StreamingContext context)
        {
            if (Img == null)
            {
                Img = new BitmapImage();
                Img.SetSource(new MemoryStream(FieldBytes));
            }
        }
    }
}
