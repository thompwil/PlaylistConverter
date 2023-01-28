using Microsoft.AspNetCore.Mvc;
using PlaylistConverter.Services;
using System.Text.Json;
using PlaylistConverter.Models;
using Microsoft.Extensions.Options;

namespace PlaylistConverter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlaylistController : ControllerBase
    {
        public readonly IOptions<Keys> _keys; 
        public PlaylistController(IOptions<Keys> keys)
        {
            _keys = keys;
        }
        [HttpPost]
        public async Task<string> GetPlaylist(PlaylistId playlistId)
        {
            SpotifyServices.DevToken = _keys.Value.SpotifyDevToken;
            PlaylistDetails playlistDetails = await SpotifyServices.GetPlaylistDetails(playlistId.Id);
            SpotifyPlaylist spotifyPlaylist = await SpotifyServices.GetPlaylistTracks(playlistId.Id);
            Playlist playlistUpdated = SpotifyServices.BuildPlaylistData(spotifyPlaylist, playlistDetails);
            return JsonSerializer.Serialize(playlistUpdated);
        }

        [HttpGet]
        public string getAppleDevToken()
        {
            AppleMusicServices.Secret = _keys.Value.AppleSecret;
            return JsonSerializer.Serialize(AppleMusicServices.CreateDevToken());
        }

    }
}
