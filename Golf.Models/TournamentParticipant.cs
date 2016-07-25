using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    public class TournamentParticipant
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("player")]
        public Player Player { get; set; }

        [JsonProperty("teeBoxId")]
        public string TeebBoxId { get; set; }

        [JsonProperty("teaTime")]
        public DateTime TeaTime { get; set; }
    }
}