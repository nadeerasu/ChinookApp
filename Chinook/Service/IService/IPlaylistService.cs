using Chinook.ClientModels;
using Chinook.Models;

namespace Chinook.Service.IService
{
    public interface IPlaylistService
    {
        Task<Playlist?> GetPlaylistAsync(long playlistId);
        Task FavoriteTrackAsync(long trackId, string userId);
        Task UnfavoriteTrackAsync(long trackId, string userId);
        Task RemoveTrackAsync(long playlistId, long trackId);
    }
}
