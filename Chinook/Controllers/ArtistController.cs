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

        [HttpGet("artists")]
        public async Task<ActionResult<IEnumerable<ArtistDTO>>> GetArtistsAsync()
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ArtistDTO>> GetArtistsByIdAsync(int id)
        {
            try
            {
                var artists = await _artistService.GetArtistByIdAsync(id);
                return Ok(artists?.ToDTO());
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }
    }
}