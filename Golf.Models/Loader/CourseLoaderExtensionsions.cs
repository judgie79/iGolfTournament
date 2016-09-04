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
    public static class CourseLoaderExtensionsions
    {
        public static async Task<Course> LoadCourse(this GolfLoader loader, string id)
        {
            return await loader.LoadAsync<Course>("courses/" + id);
        }

        public static async Task<CourseCollection> LoadCoursesOfClub(this GolfLoader loader, string clubId)
        {
            return await loader.LoadAsync<CourseCollection>("clubs/" + clubId + "/courses");
        }

        public static async Task<string> CreateCourse(this GolfLoader loader, Course course)
        {
            return await loader.PostAsync<Course, string>("courses/", course);
        }

        public static async Task<Course> UpdateCourse(this GolfLoader loader, Course course)
        {
            return await loader.PutAsync<Course, Course>("courses/" + course.Id, course);
        }

        public static async Task<bool> DeleteCourse(this GolfLoader loader, string id)
        {
            return await loader.DeleteAsync<Course>("courses/" + id);
        }
    }

    public static class TeeboxLoaderExtensionsions
    {
        public static async Task<TeeBox> LoadTeebox(this GolfLoader loader, string clubId, string courseId, string id)
        {
            return await loader.LoadAsync<TeeBox>("courses/" + courseId + "/teeboxes/" + id);
        }

        public static async Task<TeeboxCollection> LoadTeeboxesOfCourse(this GolfLoader loader, string clubId, string courseId)
        {
            return await loader.LoadAsync<TeeboxCollection>("courses/" + courseId + "/teeboxes");
        }

        public static async Task<string> CreateTeebox(this GolfLoader loader, string clubIb, string courseId, TeeBox teebox)
        {
            return await loader.PostAsync<TeeBox, string>("courses/" + courseId + "/teeboxes", teebox);
        }

        public static async Task<TeeBox> UpdateTeebox(this GolfLoader loader, string clubIb, string courseId, TeeBox teebox)
        {
            return await loader.PutAsync<TeeBox, TeeBox>("courses/" + courseId + "/teeboxes/" + teebox.Id, teebox);
        }

        public static async Task<bool> DeleteTeebox(this GolfLoader loader, string clubIb, string courseId, string id)
        {
            return await loader.DeleteAsync<TeeBox>("courses/" + courseId + "/teeboxes/" + id);
        }
    }
}