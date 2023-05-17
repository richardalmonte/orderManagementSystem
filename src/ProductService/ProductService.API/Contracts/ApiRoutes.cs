namespace ProductService.API.Contracts;

public static class ApiRoutes
{
    private const string Root = "api";
    private const string Version = "v1";

    public const string Base = Root + "/" + Version;

    public static class Products
    {
        public const string Create = Base + "/Products";
        public const string Update = Base + "/Products/{productId}";
        public const string GetAll = Base + "/Products";
        public const string Get = Base + "/Products/{productId}";
        public const string Delete = Base + "/Products/{productId}";
        public const string GetByCategoryName = Base + "/Products/Category/{categoryName}";
    }

    public static class Categories
    {
        public const string Create = Base + "/Categories";
        public const string Update = Base + "/Categories/{categoryId}";
        public const string GetAll = Base + "/Categories";
        public const string Get = Base + "/Categories/{categoryId}";
        public const string Delete = Base + "/Categories/{categoryId}";
    }
}