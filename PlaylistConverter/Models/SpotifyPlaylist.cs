namespace PlaylistConverter.Models
{
    public class SpotifyPlaylist
    {
        public IEnumerable<SpotifyPlaylistTrack> items { get; set; }
    }

    public class SpotifyPlaylistTrack
    {
        public SpotifyTrack track { get; set; }
    }

    public class SpotifyTrack
    {
        public string name { get; set; }
        public IEnumerable<SpotifyArtist> artists { get; set; }
    }

    public class SpotifyArtist
    {
        public string name { get; set; }
    }

    public class PlaylistId
    {
        public string Id { get; set; }
    }
}

