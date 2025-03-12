using Mandat.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mandat.Api.Controllers;

[ApiController]
[Route("api/match-requests")]
[Authorize]
public sealed class MatchRequestsController(MatchRequestService requests) : ControllerBase
{
    public sealed record CreateMatchRequest(Guid StudentId, Guid MentorId);

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateMatchRequest body, CancellationToken ct)
    {
        try
        {
            var created = await requests.RequestAsync(body.StudentId, body.MentorId, ct);
            return CreatedAtAction(nameof(Create), new { id = created.Id }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ProblemDetails { Title = "Mentor not found", Detail = ex.Message });
        }
    }
}
