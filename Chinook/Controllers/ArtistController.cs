using Chinook.ClientModels;
using Chinook.Mapping;
using Chinook.Service.IService;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Chinook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistDTO>>> GetArtists()
        {
            try
            {
                var artists = await _artistService.GetArtistsAsync();
                return Ok(artists.Select(a => a.ToDTO()));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("{id}/albums")]
        public async Task<ActionResult<IEnumerable<AlbumDTO>>> GetAlbumsForArtist(int id)
        {
            try
            {
                var albums = await _artistService.GetAlbumsForArtistAsync(id);
                return Ok(albums.Select(a => a.ToDTO()));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }
    }
}