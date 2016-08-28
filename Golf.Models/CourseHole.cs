using Golf.Models;
using Golf.Tournament.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    public class CourseHole : Hole
    {
        [JsonProperty("courseId")]
        [Required]
        public string CourseId { get; set; }

        [JsonProperty("teeboxId")]
        [Required]
        public string TeeboxId { get; set; }

        [JsonProperty("distance")]
        [Required]
        [MinIntValue(0)]
        public int Distance { get; set; }

        [JsonProperty("number")]
        [Required]
        [MinIntValue(0)]
        public int Number { get; set; }

        [JsonProperty("par")]
        [Required]
        [MinIntValue(3)]
        [MaxIntValue(5)]
        public int Par { get; set; }

        [JsonProperty("hcp")]
        [Required]
        [MinIntValue(1)]
        public int Hcp { get; set; }

        [JsonProperty("frontOrBack")]
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public FrontOrBack FrontOrBack { get; set; }
    }
}