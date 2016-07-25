using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    public class Team
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("hcp")]
        public float Hcp { get; set; }

        [JsonProperty("teatime")]
        public DateTime TeaTime { get; set; }

        [JsonProperty("members")]
        public TournamentParticipantCollection Members { get; set; }
    }
}