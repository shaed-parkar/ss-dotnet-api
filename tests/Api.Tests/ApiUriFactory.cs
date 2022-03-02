namespace Api.Tests;

public static class ApiUriFactory
{
    public static class Author
    {
        private const string ApiRoot = "authors";
        public static string AddNewAuthor => $"{ApiRoot}";

        public static string GetAllAuthors()
        {
            return $"{ApiRoot}";
        }

        public static string GetAuthorById(Guid authorId)
        {
            return $"{ApiRoot}/{authorId}";
        }
    }
}