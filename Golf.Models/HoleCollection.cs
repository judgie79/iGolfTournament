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

    public class CourseHoles
    {
        private HoleCollection front = new HoleCollection();
        private HoleCollection back = new HoleCollection();

        [JsonProperty("front")]
        public HoleCollection Front
        {
            get
            {
                return front;
            }
            set
            {
                front = value ?? new HoleCollection();
            }
        }

        [JsonProperty("back")]
        public HoleCollection Back
        {
            get
            {
                return back;
            }
            set
            {
                back = value ?? new HoleCollection();
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