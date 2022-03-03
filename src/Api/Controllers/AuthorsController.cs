namespace Api.Controllers;

[Route("[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly ICommandHandler _commandHandler;
    private readonly ILogger<AuthorsController> _logger;
    private readonly IQueryHandler _queryHandler;

    public AuthorsController(IQueryHandler queryHandler, ICommandHandler commandHandler,
        ILogger<AuthorsController> logger)
    {
        _queryHandler = queryHandler;
        _commandHandler = commandHandler;
        _logger = logger;
    }

    /// <summary>
    ///     Get all authors
    /// </summary>
    /// <returns>Author details</returns>
    [HttpGet(Name = "GetAllAuthors")]
    [ProducesResponseType(typeof(List<AuthorDto>), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<List<AuthorDto>>> GetAllAuthors()
    {
        var query = new GetAllAuthorsQuery();
        var authors = await _queryHandler.Handle<GetAllAuthorsQuery, List<Author>>(query);
        _logger.LogTrace("Returning all authors");
        var authorDtos = authors.Select(author => new AuthorDto(author.Id, author.FirstName, author.LastName)).ToList();
        return Ok(authorDtos);
    }

    /// <summary>
    ///     Get an author by their ID
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
    ///     Create a new author
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

    /// <summary>
    ///     Remove an author
    /// </summary>
    /// <param name="id">Id of author to remove</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}", Name = "RemoveAuthorById")]
    [ProducesResponseType(typeof(AuthorDto), (int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]
    public async Task<ActionResult<AuthorDto>> RemoveAuthorById(Guid id)
    {
        var command = new RemoveAuthorCommand(id);
        await _commandHandler.Handle(command);
        _logger.LogTrace("Removed author {AuthorId}", id);
        return Ok();
    }
}