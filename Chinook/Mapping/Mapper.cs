using Chinook.ClientModels;
using Chinook.Models;
using Chinook.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Chinook.Mapping
{
    public static class Mapper
    {
        public static ArtistDTO ToDTO(this Artist artist)
        {
            return new ArtistDTO
            {
                ArtistId = artist.ArtistId,
                Name = artist?.Name ?? string.Empty,
                AlbumCount = artist?.Albums?.Count() ?? 0
            };
        }

        public static AlbumDTO ToDTO(this Album album)
        {
            return new AlbumDTO
            {
                AlbumId = album.AlbumId,
                Title = album.Title,
                ArtistId = album.ArtistId
            };
        }

        public static PlaylistDTO ToDTO(this Playlist playlist)
        {
            return new PlaylistDTO
            {
                Name = playlist.Name,
                PlaylistId = playlist.PlaylistId,
            };
        }

        public static PlaylistTrackDTO ToDTO(this Track track, string CurrentUserId)
        {
            return new PlaylistTrackDTO
            {
                AlbumTitle = (track.Album == null ? "-" : track.Album.Title),
                TrackId = track.TrackId,
                TrackName = track.Name,
                IsFavorite = track.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == CurrentUserId && up.Playlist.Name == SD.AutomaticPlaylist)).Any()

            };
        }

        public static PlaylistDTO ToDTO(this Playlist playlist, string currentUserId)
        {
            var playlistDTO =  new PlaylistDTO
            {
                Name = playlist?.Name ?? string.Empty,
                Tracks = playlist?.Tracks?.Select(t => new PlaylistTrackDTO
                {
                    TrackId = t.TrackId,
                    TrackName = t.Name,
                    AlbumTitle = t.Album?.Title ?? string.Empty,
                    ArtistName = t.Album?.Artist.Name ?? string.Empty,
                    IsFavorite = t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == currentUserId && up.Playlist.Name == SD.AutomaticPlaylist)).Any()
                }).ToList() ?? new List<PlaylistTrackDTO>()
            };
            return playlistDTO; 
        }
    }
}
