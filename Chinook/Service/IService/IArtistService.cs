using Chinook.Models;

namespace Chinook.Service.IService
{
    public interface IArtistService
    {
        Task<List<Artist>> GetArtistsAsync();
        Task<List<Album>> GetAlbumsForArtistAsync(int artistId);
    }
}
