using Chinook.ClientModels;
using Chinook.Models;

namespace Chinook.Mapping
{
    public static class Mapper
    {
        public static ArtistDTO ToDTO(this Artist artist)
        {
            return new ArtistDTO
            {
                ArtistId = artist.ArtistId,
                Name = artist.Name,
                AlbumCount = artist.Albums?.Count() ?? 0
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
    }
}
