
using Chinook.ClientModels;
using Chinook.Mapping;
using Chinook.Service;
using Chinook.Service.IService;
using Chinook.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Chinook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        [HttpGet("{playlistId:long}")]
        public async Task<ActionResult<PlaylistDTO>> GetPlaylistAsync(long playlistId, [FromQuery] string? userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = SD.UserNotAuthorized });
                }

                var playlist = await _playlistService.GetPlaylistAsync(playlistId);
                if (playlist == null)
                {
                    return NotFound(new { message = SD.PlaylistNotFound });
                }

                var playlistDto = playlist.ToDTO(userId);
                return Ok(playlistDto);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost("favorite/{trackId:long}")]
        public async Task<IActionResult> FavoriteTrackAsync(long trackId, [FromQuery] string? userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = SD.UserNotAuthorized });
                }

                await _playlistService.AddTrackAsync(trackId, userId, SD.AutomaticPlaylist);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost()]
        public async Task<IActionResult> AddPlaylistAsync([FromQuery] string playlistName, [FromQuery] string? userId, [FromQuery] long trackId)
        {

            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = SD.UserNotAuthorized });
                }

                await _playlistService.AddTrackAsync(trackId, userId, playlistName);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost("unfavorite/{trackId:long}")]
        public async Task<IActionResult> UnfavoriteTrackAsync(long trackId, [FromQuery] string? userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = SD.UserNotAuthorized });
                }

                await _playlistService.RemoveTrackAsync(trackId, userId, SD.AutomaticPlaylist);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("playlists")]
        public async Task<ActionResult<IEnumerable<PlaylistDTO>>> GetPlaylistsAsync([FromQuery] string? userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = SD.UserNotAuthorized });
                }

                var playlists = await _playlistService.GetPlaylistsAsync(userId);
                return Ok(playlists.Select(a => a.ToDTO()));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        [HttpDelete("{playlistId:long}/track/{trackId:long}")]
        public async Task<IActionResult> RemoveTrackAsync(long playlistId, long trackId)
        {
            try
            {
                await _playlistService.RemoveTrackAsync(playlistId, trackId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }
    }
}