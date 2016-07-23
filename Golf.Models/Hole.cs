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
        public string HoleId { get; set; }

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
    }

    [JsonArray]
    public class HoleCollection: List<Hole>
    {
        public HoleCollection()
            : base()
        {
        }

        public HoleCollection(IEnumerable<Hole> holeCollection)
            : base(holeCollection)
        {

        }
    }

    public static class HoleCollectionExtensions
    {
        public static HoleCollection ToHoleCollection(this IEnumerable<Hole> holes)
        {
            return new HoleCollection(holes);
        }
    }
}