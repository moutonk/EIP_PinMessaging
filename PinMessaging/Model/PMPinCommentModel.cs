using System.ComponentModel;
using Newtonsoft.Json;
using PinMessaging.Utils;

namespace PinMessaging.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PMPinCommentModel
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
