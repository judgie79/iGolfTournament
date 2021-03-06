﻿using Golf.Tournament.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
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
}