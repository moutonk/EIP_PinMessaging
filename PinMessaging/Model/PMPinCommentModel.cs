using System;
using System.ComponentModel;
using Newtonsoft.Json;
using PinMessaging.Utils;

namespace PinMessaging.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PMPinCommentModel
    {
        [JsonProperty] [DefaultValue(null)] public string Id { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Content { get; set; }
        [JsonProperty] [DefaultValue(PMPinModel.PinsContentType.Text)] public PMPinModel.PinsContentType ContentType { get; set; }
        [JsonProperty] [DefaultValue(null)] public string CreatedTime { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Author { get; set; }
        [JsonProperty] [DefaultValue(null)] public string ModifiedTime { get; set; }
        [JsonProperty] [DefaultValue(null)] public string AuthorId { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Up { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Down { get; set; }

        public void ShowPinCommentContent()
        {
            try
            {
                Logs.Output.ShowOutput("id:" + Id + " content:" + Content + " contentType:" + ContentType +
                               " CreatedTime:" + CreatedTime + " author:" + Author + " modifiedTime:" + ModifiedTime +
                               " authorId:" + AuthorId + " up:" + Up + " down:" + Down);
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("ShowPinCommentContent: invalid content: " + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
            }
        }
    }
}
