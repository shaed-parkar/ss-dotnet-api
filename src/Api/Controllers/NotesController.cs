namespace SS.Controllers;

[Route("Authors")]
[Produces("application/json")]
[Consumes("application/json")]
public class NotesController : ControllerBase
{
    private readonly ICommandHandler _commandHandler;
    private readonly ILogger<NotesController> _logger;
    private readonly IQueryHandler _queryHandler;

    public NotesController(IQueryHandler queryHandler, ICommandHandler commandHandler,
        ILogger<NotesController> logger)
    {
        _queryHandler = queryHandler;
        _commandHandler = commandHandler;
        _logger = logger;
    }

    /// <summary>
    ///     Get all notes for author
    /// </summary>
    /// <returns>All notes for an author</returns>
    [HttpGet("{authorId:guid}/Notes", Name = "GetAllNotesForAuthor")]
    [ProducesResponseType(typeof(List<NoteDto>), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<List<NoteDto>>> GetAllNotesForAuthor(Guid authorId)
    {
        var query = new GetAllNotesForAuthorQuery(authorId);
        var notes = await _queryHandler.Handle<GetAllNotesForAuthorQuery, IReadOnlyCollection<Note>>(query);
        _logger.LogTrace("Returning all notes for author {AuthorId}", authorId);
        var noteDtos = notes.Select(note => new NoteDto(note.Content, (Priority) note.Priority)).ToList();
        return Ok(noteDtos);
    }

    /// <summary>
    ///     Create a new author
    /// </summary>
    /// <param name="authorId">UUID of author</param>
    /// <param name="newNote"></param>
    /// <returns></returns>
    [HttpPost("{authorId:guid}/Notes", Name = "AddNewNote")]
    [ProducesResponseType(typeof(NoteDto), (int) HttpStatusCode.Created)]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public async Task<ActionResult<NoteDto>> AddNewAuthor([FromRoute] Guid authorId, [FromBody] NewNoteDto newNote)
    {
        var command = new AddNoteCommand(authorId, newNote.Message, (PriorityLevel) newNote.Priority);

        await _commandHandler.Handle(command);

        var note = command.CreatedNote;
        var noteDto = new NoteDto(note.Content, (Priority) note.Priority);
        _logger.LogTrace("Created a new note for author {AuthorId}", authorId);
        return CreatedAtAction(nameof(GetAllNotesForAuthor), new {authorId}, noteDto);
    }
}