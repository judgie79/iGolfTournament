using Golf.Tournament.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    public class Membership
    {
        [JsonProperty("clubNr")]
        public string ClubNr { get; set; }

        [JsonProperty("nr")]
        public string Nr { get; set; }

        [JsonProperty("serviceNr")]
        public string ServiceNr { get; set; }
    }
}