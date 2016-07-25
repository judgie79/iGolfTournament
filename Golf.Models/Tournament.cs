using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    public class Tournament
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        [Required]
        public string Title { get; set; }

        [JsonProperty("participants")]
        public TournamentParticipantCollection Participants { get; set; }

        [JsonProperty("club")]
        [Required]
        public Club Club { get; set; }

        [JsonProperty("course")]
        [Required]
        public Course Course { get; set; }

        [JsonProperty("date")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
    }
}