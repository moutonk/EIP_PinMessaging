using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
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

        [JsonProperty] [DefaultValue(null)] Dictionary<string, string> Location { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Content { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Date { get; set; }
        [JsonProperty] [DefaultValue(PMPinModel.PinsType.PublicMessage)] public PMPinModel.PinsType PinType { get; set; }
        [JsonProperty] [DefaultValue(HistoryType.CreatePin)] public HistoryType  historyType { get; set; }

        [DefaultValue(null)] public string Id { get; set; }
        [DefaultValue(null)] public double? Latitude { get; set; }
        [DefaultValue(null)] public double? Longitude { get; set; }
        [DefaultValue(null)] public string Name { get; set; }

        [OnDeserialized]
        private void CompleteDataMembers()
        {
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
