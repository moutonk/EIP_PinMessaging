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
    public class PMGradeModel
    {
        public enum GradeType
        {
            PointBronze = 0,    
   	        PointArgent,
   	        PointOr,
    	    Pin50,
    	    Message50,
    	    Betatester         
        }

        [JsonProperty] [DefaultValue(null)] public string Name { get; set; }
        [JsonProperty] [DefaultValue(null)] public GradeType Type { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Description { get; set; }

        public void ShowGradeContent()
        {
            try
            {
                Logs.Output.ShowOutput((Name == null ? "" : "name:" + Name) +
                                       " type:" + Type +
                                       (Description == null ? "" : " description:" + Description));
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("Grade content is incorrectly formatted", Logs.Error.ErrorsPriority.NotCritical);
            }
        }
    }
}
