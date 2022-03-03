namespace SS;

public static class DefaultBuilderSettings
{
    private static BuilderSettings _instance;

    public static BuilderSettings Instance()
    {
        if (_instance != null) return _instance;

        _instance = new BuilderSettings();
        _instance.DisablePropertyNamingFor<Note, int>(note => note.Id);

        return _instance;
    }
}