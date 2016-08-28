using Golf.Tournament.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
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