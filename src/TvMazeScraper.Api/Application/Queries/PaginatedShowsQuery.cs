using System;
using System.Collections.Generic;
using MediatR;
using TvMazeScraper.Api.Dto;

namespace TvMazeScraper.Api.Application.Queries
{
    /// <summary>
    /// Get a paginated list of tv shows
    /// </summary>
    public class PaginatedShowsQuery : IRequest<IList<Show>>
    {
        /// <summary>
        /// Taske N rows in result
        /// </summary>
        public int Limit { get; }

        /// <summary>
        /// Skip first N rows
        /// </summary>
        public int Offset { get; }

        public PaginatedShowsQuery(int limit, int offset)
        {
            // Simple rules, no need to use smth complicated e.g. FluentValidation yet

            if (limit <= 0)
                throw new ArgumentException($"{nameof(limit)} must be greater than 0");

            if (limit > 100)
                throw new ArgumentException($"{nameof(limit)} must be less or equal than 100");

            if (offset < 0)
                throw new ArgumentException($"{nameof(offset)} must be greater or equal than 0");
            
            Limit = limit;
            Offset = offset;
        }
    }
}
