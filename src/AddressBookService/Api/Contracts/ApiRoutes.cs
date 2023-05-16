namespace AddressBookService.Api.Contracts;

public static class ApiRoutes
{
    private const string Root = "api";
    private const string Version = "v1";

    public const string Base = Root + "/" + Version;

    public static class Addresses
    {
        public const string Create = Base + "/Addresses";
        public const string Update = Base + "/Addresses/{addressId}";
        public const string GetAll = Base + "/Addresses";
        public const string Get = Base + "/Addresses/{addressId}";
        public const string Delete = Base + "/Addresses/{addressId}";
    }
}