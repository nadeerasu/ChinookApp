using Chinook.Models;
using Chinook.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Service
{
    public class ArtistService : IArtistService
    {
        private readonly IDbContextFactory<ChinookContext> _dbFactory;

        public ArtistService(IDbContextFactory<ChinookContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<Artist>> GetArtistsAsync()
        {
            using var dbContext = await _dbFactory.CreateDbContextAsync();
            return await dbContext.Artists.Include(a => a.Albums).ToListAsync();
        }

        public async Task<List<Album>> GetAlbumsForArtistAsync(int artistId)
        {
            using var dbContext = await _dbFactory.CreateDbContextAsync();
            return await dbContext.Albums.Where(a => a.ArtistId == artistId).ToListAsync();
        }
    }
}