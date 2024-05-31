using Chinook.ClientModels;
using Chinook.Mapping;
using Chinook.Models;
using Chinook.Service.IService;
using Chinook.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Chinook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackController : ControllerBase
    {
        private readonly ITrackService _trackService;

        public TrackController(ITrackService trackService)
        {
            _trackService = trackService;
        }

        [HttpGet("{artistId:long}")]
        public async Task<ActionResult<IEnumerable<PlaylistTrackDTO>>> GetTracks(long artistId, [FromQuery] string? userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = SD.UserNotAuthorized });
                }
                var tracks = await _trackService.GetTracksAsync(artistId);
                return Ok(tracks.Select(a => a.ToDTO(userId)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

    }
}
