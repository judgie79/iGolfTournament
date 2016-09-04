
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Golf.Tournament.Models
{
    [JsonArray]
    public class CourseCollection : List<Course>, IEnumerable<Course>
    {
        public CourseCollection()
            : base()
        {
        }

        public CourseCollection(IEnumerable<Course> courseCollection)
            : base(courseCollection)
        {

        }
    }
}
