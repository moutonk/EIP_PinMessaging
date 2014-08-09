using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Windows.Storage.Streams;
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
        public void CreateStream(StreamingContext context)
        {
            if (Img == null)
            {
                Img = new BitmapImage();
                Img.SetSource(new MemoryStream(FieldBytes));
            }
        }
    }
}
