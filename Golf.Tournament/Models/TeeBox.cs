using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    public class TeeBox
    {
        [JsonProperty("color")]
        [Required]
        public Color Color { get; set; }

        [JsonProperty("name")]
        [Required]
        public string Name { get; set; }

        [JsonProperty("distance")]
        [Required]
        public int Distance { get; set; }

        [JsonProperty("par")]
        [Required]
        public int Par { get; set; }

        [JsonProperty("courseRating")]
        [Required]
        public float CourseRating { get; set; }

        [JsonProperty("slopeRating")]
        [Required]
        public float SlopeRating { get; set; }

        [JsonProperty("holes")]
        public IEnumerable<Hole> Holes { get; set; }
    }
}