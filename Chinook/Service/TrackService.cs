using Chinook.Models;
using Chinook.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Service
{
    public class TrackService : ITrackService
    {
        private readonly IDbContextFactory<ChinookContext> _dbFactory;

        public TrackService(IDbContextFactory<ChinookContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<Track>> GetTracksAsync(long artistId)
        {
            using var dbContext = await _dbFactory.CreateDbContextAsync();
            return await dbContext.Tracks.Where(a => a.Album.ArtistId == artistId)
                .Include(t => t.Playlists)
                .ThenInclude(t => t.UserPlaylists)
                .Include(a => a.Album).ToListAsync();
        }
    }
}