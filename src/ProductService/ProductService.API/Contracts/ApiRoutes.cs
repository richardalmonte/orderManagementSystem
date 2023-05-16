namespace ProductService.API.Contracts;

public static class ApiRoutes
{
    private const string Root = "api";
    private const string Version = "v1";

    public const string Base = Root + "/" + Version;

    public static class Products
    {
        public const string Create = Base + "/Products";
        public const string Update = Base + "/Products/{ProductId}";
        public const string GetAll = Base + "/Products";
        public const string Get = Base + "/Products/{ProductId}";
        public const string Delete = Base + "/Products/{ProductId}";
    }

}