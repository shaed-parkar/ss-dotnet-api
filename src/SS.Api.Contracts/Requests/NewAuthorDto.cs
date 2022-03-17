namespace SS.Requests;

/// <summary>
///     Details for a new author
/// </summary>
/// <param name="FirstName">First name of author</param>
/// <param name="LastName">Last name of author</param>
public record NewAuthorDto(string FirstName, string LastName);

/// <summary>
///     Details for a new note
/// </summary>
/// <param name="Message">The content of a note</param>
/// <param name="Priority">The priority level</param>
public record NewNoteDto(string Message, Priority Priority);