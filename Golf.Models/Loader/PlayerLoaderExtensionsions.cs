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
    public static class PlayerLoaderExtensionsions
    {
        public static async Task<Player> LoadPlayer(this GolfLoader loader, string id)
        {
            return await loader.LoadAsync<Player>("players/" + id);
        }

        public static async Task<PlayerCollection> LoadPlayers(this GolfLoader loader)
        {
            return await loader.LoadAsync<PlayerCollection>("players");
        }

        public static async Task<string> CreatePlayer(this GolfLoader loader, Player player)
        {
            return await loader.PostAsync<Player, string>("players/", player);
        }

        public static async Task<Player> UpdatePlayer(this GolfLoader loader, Player player)
        {
            return await loader.PutAsync<Player, Player>("players/" + player.Id, player);
        }

        public static async Task<bool> DeletePlayer(this GolfLoader loader, string id)
        {
            return await loader.DeleteAsync<Player>("players/" + id);
        }
    }
}