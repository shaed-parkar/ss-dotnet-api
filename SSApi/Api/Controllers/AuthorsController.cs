using System.Net;
using Microsoft.AspNetCore.Mvc;
using SS.Api.Contracts;
using SS.DAL.Commands.Core;
using SS.DAL.Queries;
using SS.DAL.Queries.Core;
using SS.Domain;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IQueryHandler _queryHandler;
        private readonly ICommandHandler _commandHandler;
        private readonly ILogger<AuthorsController> _logger;
        
        // GET: api/Authors
        public AuthorsController(IQueryHandler queryHandler, ICommandHandler commandHandler, ILogger<AuthorsController> logger)
        {
            _queryHandler = queryHandler;
            _commandHandler = commandHandler;
            _logger = logger;
        }

        /// <summary>
        /// Get an author by their ID
        /// </summary>
        /// <param name="id">Author ID</param>
        /// <returns>Author details</returns>
        [HttpGet("{id:guid}", Name = "GetAuthorById")]
        [ProducesResponseType(typeof(AuthorDto), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<AuthorDto>> GetAuthorById(Guid id)
        {
            var query = new GetAuthorByIdQuery(id);
            var author = await _queryHandler.Handle<GetAuthorByIdQuery, Author?>(query);
            if (author is null)
            {
                _logger.LogWarning("Unable to find an author {AuthorId}", id);
                return NotFound();
            }
            _logger.LogTrace("Found author {AuthorId}", id);
            return Ok(new AuthorDto(author.Id, author.FirstName, author.LastName));
        }
    }
}
