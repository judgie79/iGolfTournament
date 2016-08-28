using Golf.Tournament.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    public static class CourseHoleCollectionExtensions
    {
        public static CourseHoleCollection ToCourseHoleCollection(this IEnumerable<CourseHole> holes)
        {
            return new CourseHoleCollection(holes);
        }
    }
}