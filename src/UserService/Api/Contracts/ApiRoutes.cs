namespace UserService.Contracts;

public static class ApiRoutes
{
    private const string Root = "api";
    private const string Version = "v1";

    public const string Base = Root + "/" + Version;

    public static class Users
    {
        public const string Create = Base + "/users";
        public const string Update = Base + "/users/{userId}";
        public const string GetAll = Base + "/users";
        public const string Get = Base + "/users/{userId}";
        public const string Delete = Base + "/users/{userId}";
    }
}