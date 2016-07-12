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
        public string ClubId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("teeboxes")]
        //public TeeboxCollection TeeBoxes { get; set; }
        public IEnumerable<TeeBox> TeeBoxes { get; set; }
    }

    public class TeeboxCollection : List<TeeBox>
    {

    }
}