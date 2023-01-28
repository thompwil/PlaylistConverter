using Microsoft.AspNetCore.Mvc;
using PlaylistConverter.Services;
using System.Text.Json;
using PlaylistConverter.Models;

namespace PlaylistConverter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlaylistController : ControllerBase
    { 
        [HttpPost]
        public async Task<string> GetPlaylist(PlaylistId playlistId)
        {
            PlaylistDetails playlistDetails = await SpotifyServices.GetPlaylistDetails(playlistId.Id);
            Playlist playlist = await SpotifyServices.GetPlaylistTracks(playlistId.Id);
            Playlist playlistUpdated = SpotifyServices.GetSongs(playlist, playlistDetails);
            return JsonSerializer.Serialize(playlistUpdated);
        }

        [HttpGet]
        public async Task<string> getAppleDevToken()
        {
            return JsonSerializer.Serialize(await AppleMusicServices.CreateDevToken());
        }

        public class PlaylistId
        {
            public string Id { get; set; }
        }
    }
}
