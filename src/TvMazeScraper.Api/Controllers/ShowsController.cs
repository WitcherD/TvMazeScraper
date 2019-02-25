using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TvMazeScraper.Api.Application.Queries;
using TvMazeScraper.Api.Dto;

namespace TvMazeScraper.Api.Controllers
{
    /// <summary>
    /// Enpoints for working with shows
    /// </summary>
    [Route("api/v1/shows")]
    public class ShowsController : Controller
    {
        private readonly IMediator _mediator;

        /// <inheritdoc />
        public ShowsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Provides a paginated list of all tv shows
        /// </summary>
        [HttpGet("")]
        [ProducesResponseType(typeof(IList<Show>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetShowsAsync([FromQuery] int limit, [FromQuery] int offset)
        {
            var shows = await _mediator.Send(new PaginatedShowsQuery(limit, offset));
            return Ok(shows);
        }
    }
}
