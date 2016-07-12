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
        [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")]
        public string Color { get; set; }

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
        public decimal CourseRating { get; set; }

        [JsonProperty("slopeRating")]
        [Required]
        public decimal SlopeRating { get; set; }

        [JsonProperty("holes")]
        public IEnumerable<Hole> Holes { get; set; }
    }
}