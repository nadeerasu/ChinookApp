namespace Chinook.ClientModels;

public class PlaylistDTO
{
    public string? Name { get; set; }
    public long PlaylistId { get; set; }
    public List<PlaylistTrackDTO> Tracks { get; set; }
}