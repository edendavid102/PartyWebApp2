using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpotifyAPI.Web;

namespace PartWebApp2.Services
{
    public class SpotifyClientService : ISpotifyClientService
    {
        private static readonly string SPOTIFY_CLIENT_ID = "3914517ae1724e64975d9ed0f7f08bfc";
        private static readonly string SPOTIFY_CLIENT_SECRET = "6dc9817349fa4c51961d2689e6f7c3bf";
        private readonly SpotifyClient _spotify;

        public SpotifyClientService()
        {
            var config = SpotifyClientConfig.CreateDefault()
                .WithAuthenticator(new ClientCredentialsAuthenticator(SPOTIFY_CLIENT_ID, SPOTIFY_CLIENT_SECRET));

            _spotify = new SpotifyClient(config);
        }

        public async Task<string> getArtistIdBySearchParams(string searchParams)
        {
            var searchResponse = await _spotify.Search.Item(new SearchRequest(SearchRequest.Types.Artist, searchParams)
            {
                Limit = 1,

            });
            if (searchResponse.Artists.Items.Count > 0) return searchResponse.Artists.Items.First().Id;
            else return "NO_RESULT";

        }


        public async Task<FullArtist> GetArtist(string artistId)
        {
            var artist = await _spotify.Artists.Get(artistId);
            return artist;
        }

        public async Task<List<FullArtist>> GetArtists(List<string> artistsId)
        {
            var artists = await _spotify.Artists.GetSeveral(new ArtistsRequest(artistsId));
            return artists.Artists;
        }

    }
}
