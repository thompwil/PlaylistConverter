using Jose;
using PlaylistConverter.Models;
using System.Security.Cryptography;

namespace PlaylistConverter.Services
{
    public static class AppleMusicServices
    {
        public static string Token = "";
        public async static Task Authorize()
        {
            // SSO into Apple Music
        }

        public async static void CreatePlaylist(Playlist playlist)
        {
            await Authorize();

            // Get list of songs from playlist 
            // Create Apple Music playlist
            // Return URL


        }

        public async static Task<string> CreateDevToken()
        {
            var iat = Math.Round((DateTime.UtcNow.AddMinutes(-1) - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds, 0);
            var exp = Math.Round((DateTime.UtcNow.AddMinutes(30) - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds, 0);

            var payload = new Dictionary<string, object>()
        {
            { "iat", iat },
            { "exp", exp },
            { "iss", "2LVQ52Y6V4" }
        };

            var extraHeader = new Dictionary<string, object>()
        {
            { "alg", "ES256" },
            { "typ", "JWT" },
            { "kid", "6C9TU33CXS" }
        };

            var keyString = "MIGTAgEAMBMGByqGSM49AgEGCCqGSM49AwEHBHkwdwIBAQQgrS+kt9hZQDcbTpkY5o0uPYV3zUfZIaiY4xU0nYHYJkugCgYIKoZIzj0DAQehRANCAATJiwgt5kJteru/PjjAyxF24z+sbDTutwDcJ+xVdVpOUMU2yJr18OqhT9dObvOynLOfi0wWU2n4J9Wny+E0SHXC";

            CngKey privateKey = CngKey.Import(Convert.FromBase64String(keyString), CngKeyBlobFormat.Pkcs8PrivateBlob);

            return JWT.Encode(payload, privateKey, JwsAlgorithm.ES256, extraHeader);
        }
    }
}
