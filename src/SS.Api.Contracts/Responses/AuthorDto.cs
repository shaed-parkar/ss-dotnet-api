namespace SS.Responses;

/// <summary>
///     Details for an existing author
/// </summary>
/// <param name="Id">Author's UUID</param>
/// <param name="FirstName">Author's first name</param>
/// <param name="LastName">Author's last name</param>
public record AuthorDto(Guid Id, string FirstName, string LastName);