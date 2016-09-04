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
    public static class CourseHoleLoaderExtensionsions
    {
        public static async Task<CourseHoleCollection> LoadCourseHolesOfTeebox(this GolfLoader loader, string clubId, string courseId, string teeboxId)
        {
            return await loader.LoadAsync<CourseHoleCollection>("courses/" + courseId + "/teeboxes/" + teeboxId + "/holes");
        }

        public static async Task<string> CreateHole(this GolfLoader loader, string clubId, string courseId, string teeboxId, CourseHole hole)
        {
            return await loader.PostAsync<CourseHole, string>("courses/" + courseId + "/teeboxes/" + teeboxId + "/holes", hole);
        }

        public static async Task<CourseHole> UpdateHole(this GolfLoader loader, string clubId, string courseId, string teeboxId, CourseHole hole)
        {
            return await loader.PutAsync<CourseHole, CourseHole>("courses/" + courseId + "/teeboxes/" + teeboxId + "/holes/" + hole.Id, hole);
        }

        public static async Task<bool> DeleteHole(this GolfLoader loader, string clubId, string courseId, string teeboxId, string id)
        {
            return await loader.DeleteAsync<CourseHole>("courses/" + courseId + "/teeboxes/" + teeboxId + "/holes/" + id);
        }
    }
}