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
        [JsonProperty("holeId")]
        public string HoleId { get; set; }

        [JsonProperty("number")]
        [Required]
        public int Number { get; set; }

        [JsonProperty("distance")]
        [Required]
        public int Distance { get; set; }

        [JsonProperty("par")]
        [Required]
        public int Par { get; set; }

        [JsonProperty("hcp")]
        [Required]
        public int Hcp { get; set; }
    }
}