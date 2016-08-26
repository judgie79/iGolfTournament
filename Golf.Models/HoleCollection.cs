using Golf.Tournament.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
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

    [JsonArray]
    public class CourseHoleCollection : List<CourseHole>
    {
        public CourseHoleCollection()
            : base()
        {
        }

        public CourseHoleCollection(IEnumerable<CourseHole> holeCollection)
            : base(holeCollection)
        {

        }
    }

    public class CourseHole : Hole
    {
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
    }

    public class CourseHoles
    {
        private CourseHoleCollection front = new CourseHoleCollection();
        private CourseHoleCollection back = new CourseHoleCollection();

        [JsonProperty("front")]
        public CourseHoleCollection Front
        {
            get
            {
                return front;
            }
            set
            {
                front = value ?? new CourseHoleCollection();
            }
        }

        [JsonProperty("back")]
        public CourseHoleCollection Back
        {
            get
            {
                return back;
            }
            set
            {
                back = value ?? new CourseHoleCollection();
            }
        }

        public int Count
        {
            get
            {
                return Front.Count + Back.Count;
            }
        }
    }
}