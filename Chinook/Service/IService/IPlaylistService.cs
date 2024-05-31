using Chinook.ClientModels;
using Chinook.Models;

namespace Chinook.Service.IService
{
    public interface IPlaylistService
    {
        Task<Playlist?> GetPlaylistAsync(long playlistId);
        Task<List<Playlist>> GetPlaylistsAsync(string userId);
        Task<Playlist?> GetPlaylistAsync(long playlistId, string userId);
        Task<Playlist> AddPlaylistAsync(string userId, string playlistName);
        Task AddTrackAsync(long trackId, string userId, string playlistName);
        Task addUserPlaylistToPlaylist(long playlistId, string userId, long trackId);
        Task RemoveTrackAsync(long trackId, string userId, string playlistName);
        Task RemoveTrackAsync(long trackId, string userId, long playlistId);
        Task RemoveTrackAsync(long playlistId, long trackId);
    }
}
