using Chinook.Models;

namespace Chinook.Service.IService
{
    public interface ITrackService
    {
        Task<List<Track>> GetTracksAsync(long artistId);
    }
}
