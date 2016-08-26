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

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("courseImage")]
        public string CourseImage { get; set; }
    }
}