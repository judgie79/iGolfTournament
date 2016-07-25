using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    public class HomeClub
    {
        [JsonProperty("_id")]
        [Required]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}