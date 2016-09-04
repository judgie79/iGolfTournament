using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Golf.Tournament.Controllers
{
    public static class CourseLoaderExtensions
    {
        public static async Task<IEnumerable<Course>> SetHoles(this GolfLoader loader, IEnumerable<Course> courses)
        {
            List<Task<Course>> tasks = new List<Task<Course>>();

            foreach (var course in courses)
            {
                tasks.Add(SetHoles(loader, course));
            }
            var tasksArray = tasks.ToArray();

            return await Task.WhenAll(tasksArray);
        }

        public static async Task<Course> SetHoles(this GolfLoader loader, Course course)
        {
            var holesPerTeebox = await loader.GetHolesAsync(course.Id, course.TeeBoxes.Select(t => t.Id).ToArray());

            foreach (var teebox in course.TeeBoxes)
            {
                var holes = holesPerTeebox.Single(h => h.Key == teebox.Id).Value;
                var front = holes.Where(h => h.FrontOrBack == FrontOrBack.Front);
                var back = holes.Where(h => h.FrontOrBack == FrontOrBack.Back);

                teebox.Holes = new CourseHoles()
                {
                    Front = new CourseHoleCollection(front),
                    Back = new CourseHoleCollection(back)
                };
            }

            return course;
        }

        public static async Task<KeyValuePair<string, CourseHoleCollection>[]> GetHolesAsync(this GolfLoader loader, string courseId, string[] teeboxIds)
        {
            List<Task<KeyValuePair<string, CourseHoleCollection>>> tasks = new List<Task<KeyValuePair<string, CourseHoleCollection>>>();

            foreach (var teeboxId in teeboxIds)
            {
                tasks.Add(GetHolesAsync(loader, courseId, teeboxId));
            }
            var tasksArray = tasks.ToArray();

            return await Task.WhenAll(tasksArray);
        }

        public static async Task<KeyValuePair<string, CourseHoleCollection>> GetHolesAsync(this GolfLoader loader, string courseId, string teeboxId)
        {
            return new KeyValuePair<string, CourseHoleCollection>(teeboxId, await loader.LoadAsync<CourseHoleCollection>("courses/" + courseId + "/teeboxes/" + teeboxId + "/holes"));
        }
    }

    
}