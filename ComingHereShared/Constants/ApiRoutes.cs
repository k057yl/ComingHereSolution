namespace ComingHereShared.Constants
{
    public static class ApiUrls
    {
        public const string CLIENT_ORIGIN = "https://localhost:7184";
        public const string GATEWAY_URL = "https://localhost:5001";
        public const string SERVER_URL = "https://localhost:7255";
    }

    public static class ClientRoutes
    {
        public const string LOGIN = "/login";
        public const string REGISTER = "/register";
        public const string LOGOUT = "/logout";
    }

    public static class ApiRoutes
    {
        public static class Event
        {
            public const string ACTIVE = "api/events/active";
            public const string CREATE = "api/events/create";
            public const string DELETE = "api/events/delete";
        }

        public static class Account
        {
            public const string LOGIN = "api/account/login";
            public const string CONFIRM = "api/account/confirm";
        }
    }

    public static class Roles
    {
        public const string GALA = "Gala";
        public const string USER = "User";
    }
}
