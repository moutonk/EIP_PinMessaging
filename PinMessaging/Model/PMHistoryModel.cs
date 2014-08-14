using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PinMessaging.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PMHistoryModel
    {
        public enum HistoryType
        {
            CreatePin = 0,
   	        ChangePin,
            DeletePin,
  	        CreatePinMessage,
  	        NewUser,
  	        AddFavoriteUser,
  	        AddFavoriteLocation
        }

        [JsonProperty] [DefaultValue(null)] public Dictionary<string, string> Location { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Content { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Date { get; set; }
        [JsonProperty] [DefaultValue(null)] public PMPinModel.PinsType? PinType { get; set; }
        [JsonProperty] [DefaultValue(null)] public HistoryType?  historyType { get; set; }

        [DefaultValue(null)] public string Id { get; set; }
        [DefaultValue(null)] public double? Latitude { get; set; }
        [DefaultValue(null)] public double? Longitude { get; set; }
        [DefaultValue(null)] public string Name { get; set; }

        [OnDeserialized]
        private void CompleteDataMembers(StreamingContext context)
        {
            if (Location == null)
                return;

            if (Location.ContainsKey("id"))
                Id = Location["id"];
            if (Location.ContainsKey("latitude"))
                Latitude = Utils.Utils.ConvertDoubleCommaToPoint(Location["latitude"]);
            if (Location.ContainsKey("longitude"))
                Longitude = Utils.Utils.ConvertDoubleCommaToPoint(Location["longitude"]);
            if (Location.ContainsKey("name"))
                Name = Location["name"];
        }
    }
}
