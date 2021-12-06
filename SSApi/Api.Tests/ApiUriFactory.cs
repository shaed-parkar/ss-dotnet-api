using System;

namespace Api.Tests;

public static class ApiUriFactory
{
    public static class Author
    {
        private const string ApiRoot = "authors";
        public static string GetAuthorById(Guid authorId) => $"{ApiRoot}/{authorId}";
        public static string AddNewAuthor => $"{ApiRoot}";
    }
}