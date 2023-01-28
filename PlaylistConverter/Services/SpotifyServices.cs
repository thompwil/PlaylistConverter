using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using PlaylistConverter.Models;

namespace PlaylistConverter.Services
{
    public static class SpotifyServices
    {
        public static string Token = "";
        public static string DevToken = "";
        public async static Task Authorize()
        {
            HttpClient httpClient = new HttpClient();

            using(var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token"))
            {
                
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", "YWVlOTUwYmMxNGZmNDIzOGI3MmI0ZmE1NGExNjY4NzU6YzIzYjQ2MGY0MDI3NGZlN2IzYzdhMmI4MWMwMzFiNGY=");
                var body = new Dictionary<string, string> 
                {
                    { "grant_type", "client_credentials" }
                };


                using (var content = new FormUrlEncodedContent(body))
                { 
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    requestMessage.Content = content;
                    HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
                    SpotifyToken data = await response.Content.ReadFromJsonAsync<SpotifyToken>();
                    Token = data.access_token;
                }
            }
        }

        public static async Task<PlaylistDetails> GetPlaylistDetails(string playlistId)
        {
            await Authorize();
            HttpClient httpClient = new HttpClient();

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.spotify.com/v1/playlists/" + playlistId))
            {

                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
                PlaylistDetails playlistDetails = await response.Content.ReadFromJsonAsync<PlaylistDetails>();
                return playlistDetails;
            }
        }

        public static async Task<SpotifyPlaylist> GetPlaylistTracks(string playlistId)
        {
            await Authorize();
            HttpClient httpClient = new HttpClient();

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.spotify.com/v1/playlists/" + playlistId + "/tracks"))
            {

                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);            
                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
                SpotifyPlaylist? playlist = await response.Content.ReadFromJsonAsync<SpotifyPlaylist>();
                return playlist;
            }
        }

        public static Playlist BuildPlaylistData(SpotifyPlaylist spotifyPlaylist, PlaylistDetails details)
        {
            List<Song> songs = new List<Song>();
            foreach(var item in spotifyPlaylist.items)
            {
                Song song = new Song();
                List<string> artists = new List<string>();
                song.title = item.track.name;
                foreach(var artist in item.track.artists)
                {
                    artists.Add(artist.name);
                }

                song.artists = artists;
                songs.Add(song);
                
            }

            Playlist playlist = new Playlist();
            playlist.songs = songs;
            playlist.description = details.description;
            playlist.name = details.name;
            return playlist;
        }
    }
}
