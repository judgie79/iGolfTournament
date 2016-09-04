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
    public static class ClubLoaderExtensionsions
    {
        public static async Task<Club> LoadClub(this GolfLoader loader, string id)
        {
            return await loader.LoadAsync<Club>("clubs/" + id);
        }

        public static async Task<ClubCollection> LoadClubs(this GolfLoader loader)
        {
            return await loader.LoadAsync<ClubCollection>("clubs/");
        }

        public static async Task<string> CreateClub(this GolfLoader loader, Club club)
        {
            return await loader.PostAsync<Club, string>("clubs/", club);
        }

        public static async Task<Club> UpdateClub(this GolfLoader loader, Club club)
        {
            return await loader.PutAsync<Club, Club>("clubs/" + club.Id, club);
        }

        public static async Task<bool> DeleteClub(this GolfLoader loader, string id)
        {
            return await loader.DeleteAsync<Club>("clubs/" + id);
        }
    }
}