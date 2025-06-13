namespace ComingHereShared.Constants
{
    public static class ApiRoutes
    {
        public static class Event
        {
            public const string Active = "api/event/active";
            public const string Create = "api/event/create";
            public const string Delete = "api/event/delete";
        }

        public static class Account
        {
            public const string Login = "/login";
            public const string Register = "/register";
            public const string Logout = "/logout";
            public const string Confirm = "api/account/confirm";
        }
    }
}
