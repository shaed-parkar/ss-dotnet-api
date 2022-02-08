using System.Net;
using Microsoft.AspNetCore.Mvc;
using SS.Api.Contracts;
using SS.Api.Contracts.Requests;
using SS.DAL.Commands;
using SS.DAL.Commands.Core;
using SS.DAL.Queries;
using SS.DAL.Queries.Core;
using SS.Domain;

namespace Api.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
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
            var author = await _queryHandler.Handle<GetAuthorByIdQuery, Author>(query);
            if (author is null)
            {
                _logger.LogWarning("Unable to find an author {AuthorId}", id);
                return NotFound();
            }
            _logger.LogTrace("Found author {AuthorId}", id);
            return Ok(new AuthorDto(author.Id, author.FirstName, author.LastName));
        }

        /// <summary>
        /// Create a new author
        /// </summary>
        /// <param name="newAuthor"></param>
        /// <returns></returns>
        [HttpPost(Name = "AddNewAuthor")]
        [ProducesResponseType(typeof(AuthorDto), (int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<ActionResult<AuthorDto>> AddNewAuthor([FromBody] NewAuthorDto newAuthor)
        {
            var command = new AddAuthorCommand(newAuthor.FirstName, newAuthor.LastName);
            
            await _commandHandler.Handle(command);
            
            var author = command.CreatedAuthor;
            var authorDto = new AuthorDto(author.Id, author.FirstName, author.LastName);
            _logger.LogTrace("Created a new author {AuthorId}", author.Id);
            return CreatedAtAction(nameof(GetAuthorById), new {id = author.Id}, authorDto);
        }
    }
}
