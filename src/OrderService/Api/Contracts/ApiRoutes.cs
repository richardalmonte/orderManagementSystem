namespace OrderService.Contracts;

public static class ApiRoutes
{
    private const string Root = "api";
    private const string Version = "v1";

    public const string Base = Root + "/" + Version;

    public static class Orders
    {
        public const string Create = Base + "/orders";
        public const string Update = Base + "/orders/{orderId}";
        public const string GetAll = Base + "/orders";
        public const string Get = Base + "/orders/{orderId}";
        public const string Delete = Base + "/orders/{orderId}";
    }
}