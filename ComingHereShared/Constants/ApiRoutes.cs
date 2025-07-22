namespace ComingHereShared.Constants
{
    public static class ApiUrls
    {
        public const string ClientOrigin = "https://localhost:7184";
        public const string GatewayUrl = "https://localhost:5001";
        public const string ServerUrl = "https://localhost:7255";
    }

    public static class ClientRoutes
    {
        public const string Login = "/login";
        public const string Register = "/register";
        public const string Logout = "/logout";
    }

    public static class ApiRoutes
    {
        public static class Event
        {
            public const string Active = "api/events/active";
            public const string Create = "api/events/create";
            public const string Delete = "api/events/delete";
        }

        public static class Account
        {
            public const string Login = "api/account/login";
            public const string Confirm = "api/account/confirm";
        }
    }

    public static class Roles
    {
        public const string Gala = "Gala";
        public const string User = "User";
    }
}
