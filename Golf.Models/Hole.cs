using Golf.Tournament.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    public class Hole
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("clubId")]
        public string ClubId { get; set; }

        [JsonProperty("number")]
        [Required]
        [MinIntValue(1)]
        public int Number { get; set; }

        [JsonProperty("distance")]
        [Required]
        [MinIntValue(0)]
        public int Distance { get; set; }

        [JsonProperty("par")]
        [Required]
        [MinIntValue(3)]
        [MaxIntValue(5)]
        public int Par { get; set; }

        [JsonProperty("hcp")]
        [Required]
        [MinIntValue(1)]
        public int Hcp { get; set; }

        [JsonProperty("courseImage")]
        public string CourseImage { get; set; }
    }
}