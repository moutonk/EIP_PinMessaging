using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

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
            Img = new BitmapImage();
        }

        public void CreateStream()
        {
            Img.SetSource(new MemoryStream(FieldBytes));
        }

        [OnDeserialized]
        public void CreateStream(StreamingContext context)
        {
            Img.SetSource(new MemoryStream(FieldBytes));
        }
    }
}
