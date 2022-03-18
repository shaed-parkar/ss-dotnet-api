namespace SS;

public static class ApiUriFactory
{
    public static class Author
    {
        public const string ApiRoot = "authors";
        public static string AddNewAuthor => $"{ApiRoot}";

        public static string GetAllAuthors => $"{ApiRoot}";

        public static string GetAuthorById(Guid authorId) => $"{ApiRoot}/{authorId}";

        public static string RemoveAuthorById(Guid authorId) => $"{ApiRoot}/{authorId}";
    }

    public static class Note
    {
        public const string ApiRoot = "authors";

        public static string AddNewNotes(Guid authorId) => $"{ApiRoot}/{authorId}/notes";

        public static string GetAllNotesForAuthor(Guid authorId) => $"{ApiRoot}/{authorId}/notes";
    }
}