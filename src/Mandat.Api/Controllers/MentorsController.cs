using Mandat.Application.Abstractions;
using Mandat.Application.Services;
using Mandat.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mandat.Api.Controllers;

[ApiController]
[Route("api/mentors")]
public sealed class MentorsController(MentorSearchService search) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyList<MentorSearchResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(
        [FromQuery] Subject? subject,
        [FromQuery] MeetingType? meetingType,
        [FromQuery] decimal? maxHourlyRate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var criteria = new MentorSearchCriteria
        {
            Subject = subject,
            MeetingType = meetingType,
            MaxHourlyRate = maxHourlyRate,
            Page = page,
            PageSize = pageSize
        };

        return Ok(await search.SearchAsync(criteria, ct));
    }
}
