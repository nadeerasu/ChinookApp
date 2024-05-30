namespace Chinook.ClientModels;

public class PlaylistDTO
{
    public string Name { get; set; }
    public List<PlaylistTrackDTO> Tracks { get; set; }
}