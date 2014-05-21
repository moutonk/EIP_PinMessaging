﻿
using System;
using System.ComponentModel;
using System.Windows.Interop;
using Newtonsoft.Json;
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
        [JsonProperty] [DefaultValue(null)] public string Login { get; set; }
        [JsonProperty] [DefaultValue(null)] public string SimId { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Grade { get; set; }

        public void ShowUserContent()
        {
            try
            {
                Logs.Output.ShowOutput((Id == null ? "" : "id:" + Id) + 
                                        (NbrMessage == null ? "" : " nbrMessage:" + NbrMessage) + 
                                        (Points == null ? "" : " points:" + Points) +
                                        (NbrPin == null ? "" : " nbrpin:" + NbrPin) +
                                        (Email == null ? "" : " email:" + Email) +
                                        (Login == null ? "" : " login:" + Login) +
                                        (CreatedTime == null ? "" : " createdtime:" + CreatedTime) +
                                        (Grade == null ? "" : " grade:" + Grade));
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("User content is incorrectly formatted", Logs.Error.ErrorsPriority.NotCritical);
            }
        }
    }
}
