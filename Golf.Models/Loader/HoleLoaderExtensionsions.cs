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
    public static class HoleLoaderExtensionsions
    {
        public static async Task<Hole> LoadHole(this GolfLoader loader, string clubId, string holeId)
        {
            return await loader.LoadAsync<Hole>("clubs/" + clubId + "/holes" + holeId);
        }

        public static async Task<HoleCollection> LoadHolesOfClub(this GolfLoader loader, string clubId)
        {
            return await loader.LoadAsync<HoleCollection>("clubs/" + clubId + "/holes");
        }

        public static async Task<string> CreateHole(this GolfLoader loader, string clubId, Hole hole)
        {
            return await loader.PostAsync<Hole, string>("clubs/" + clubId + "/holes/", hole);
        }

        public static async Task<Hole> UpdateHole(this GolfLoader loader, string clubId, Hole hole)
        {
            return await loader.PutAsync<Hole, Hole>("clubs/" + clubId + "/holes/" + hole.Id, hole);
        }

        public static async Task<bool> DeleteHole(this GolfLoader loader, string clubId, string id)
        {
            return await loader.DeleteAsync<Hole>("clubs/" + clubId + "/holes/" + id);
        }
    }
}