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
        public TeeBox()
        {
            Holes = new CourseHoles();
            Color = new Color("#ffffff");
        }

        [JsonProperty("_id")]
        public string Id { get; set; }

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
        
        private CourseHoles holes = new CourseHoles();

        [JsonProperty("holes")]
        public CourseHoles Holes
        {
            get
            {
                return holes;
            }
            set
            {
                holes = value ?? new CourseHoles();
            }
        }
    }
}