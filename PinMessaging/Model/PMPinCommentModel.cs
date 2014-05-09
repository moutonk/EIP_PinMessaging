using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PinMessaging.Utils;

namespace PinMessaging.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    class PMPinCommentModel
    {
        [JsonProperty] [DefaultValue(null)] public string Id { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Up { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Down { get; set; }
        [JsonProperty] [DefaultValue(null)] public PMPinModel Message { get; set; }

        public void ShowPinContent()
        {
            Logs.Output.ShowOutput("id: " + Id + " up: " + Up + " down: " + Down);
            Message.ShowPinContent();
        }
    }
}
