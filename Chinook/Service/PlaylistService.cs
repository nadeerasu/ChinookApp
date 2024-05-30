using Chinook.ClientModels;
using Chinook.Models;
using Chinook.Service.IService;
using Chinook.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Chinook.Service
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IDbContextFactory<ChinookContext> _dbFactory;

        public PlaylistService(IDbContextFactory<ChinookContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<Playlist?> GetPlaylistAsync(long playlistId)
        {
            using var dbContext = await _dbFactory.CreateDbContextAsync();
            var playlist = await dbContext.Playlists
                .Include(n => n.UserPlaylists)
                .Include(p => p.Tracks)
                .ThenInclude(t => t.Album)
                .ThenInclude(a => a.Artist)
                .FirstOrDefaultAsync(p => p.PlaylistId == playlistId);

            return playlist;
        }

        public async Task FavoriteTrackAsync(long trackId, string userId)
        {
            using var dbContext = await _dbFactory.CreateDbContextAsync();
            // Check if the user's favorite playlist exists
            var favoritePlaylist = await dbContext.Playlists
                .Include(p => p.UserPlaylists)
                .Include(p => p.Tracks)  // Ensure Tracks are included
                .FirstOrDefaultAsync(p => p.UserPlaylists.Any(up => up.UserId == userId && up.Playlist.Name == SD.AutomaticPlaylist));
            if (favoritePlaylist == null)
            {
                var maxPlaylistId = await dbContext.Playlists.MaxAsync(p => (long?)p.PlaylistId) ?? 0;
                var newPlaylistId = maxPlaylistId + 1;
                // Create a new favorite playlist for the user if it doesn't exist
                favoritePlaylist = new Playlist
                {
                    Name = SD.AutomaticPlaylist,
                    UserPlaylists = new List<UserPlaylist>
                    {
                        new UserPlaylist { UserId = userId },

                    },
                    PlaylistId = newPlaylistId

                };
                dbContext.Playlists.Add(favoritePlaylist);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                // Check if the track is already in the playlist
                var track = favoritePlaylist.Tracks.FirstOrDefault(t => t.TrackId == trackId);
                if (track != null)
                {
                    return;
                }
            }

            // Add the track to the playlist
            var trackToAdd = await dbContext.Tracks.FirstOrDefaultAsync(t => t.TrackId == trackId);
            if (trackToAdd != null)
            {
                favoritePlaylist.Tracks.Add(trackToAdd);
                await dbContext.SaveChangesAsync();
            }

            //Ensure the UserPlaylist relationship is maintained
            if (!favoritePlaylist.UserPlaylists.Any(up => up.UserId == userId))
            {
                var userPlaylist = new UserPlaylist
                {
                    UserId = userId,
                    Playlist = favoritePlaylist
                };
                dbContext.UserPlaylists.Add(userPlaylist);
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task UnfavoriteTrackAsync(long trackId, string userId)
        {
            using var dbContext = await _dbFactory.CreateDbContextAsync();
            var userPlaylist = await dbContext.UserPlaylists
                .FirstOrDefaultAsync(up => up.UserId == userId && up.Playlist.Name == SD.AutomaticPlaylist && up.Playlist.Tracks.Any(x => x.TrackId == trackId));
            if (userPlaylist != null)
            {
                await RemoveTrackAsync(userPlaylist.PlaylistId, trackId);
            }
        }

        public async Task RemoveTrackAsync(long playlistId, long trackId)
        {
            using var dbContext = await _dbFactory.CreateDbContextAsync();
            var playlist = await dbContext.Playlists
                .Include(p => p.Tracks)
                .FirstOrDefaultAsync(p => p.PlaylistId == playlistId);

            if (playlist != null)
            {
                var track = playlist.Tracks.FirstOrDefault(t => t.TrackId == trackId);
                if (track != null)
                {
                    playlist.Tracks.Remove(track);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}