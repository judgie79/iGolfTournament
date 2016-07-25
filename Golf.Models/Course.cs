using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    public class Course
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("clubId")]
        [Required]
        public string ClubId { get; set; }

        [JsonProperty("name")]
        [Required]
        public string Name { get; set; }

        private TeeboxCollection teeboxes = new TeeboxCollection();
        [JsonProperty("teeboxes")]
        public TeeboxCollection TeeBoxes
        {
            get
            {
                return teeboxes;
            }
            set
            {
                teeboxes = value ?? new TeeboxCollection();
            }
        }
    }
}