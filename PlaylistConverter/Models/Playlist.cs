namespace PlaylistConverter.Models
{
    public class Playlist
    {
        public IEnumerable<PlaylistTrack> items { get; set; }
        public IEnumerable<Song> songs { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
    }
}
