namespace SS.Api.Contracts.Requests;

/// <summary>
///     Details for a new author
/// </summary>
/// <param name="FirstName">First name of author</param>
/// <param name="LastName">Last name of author</param>
public record NewAuthorDto(string FirstName, string LastName);