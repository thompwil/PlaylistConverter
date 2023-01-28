namespace PlaylistConverter.Models
{
    public class Song
    {
        public string title { get; set; }
        public IEnumerable<string> artists { get; set; }
    }
}
