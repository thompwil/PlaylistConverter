namespace PlaylistConverter.Models
{
    public class Track
    {
        public string name { get; set; }
        public IEnumerable<Artist> artists { get; set; }
    }
}
