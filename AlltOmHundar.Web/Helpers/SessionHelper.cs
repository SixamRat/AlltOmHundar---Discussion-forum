using Microsoft.AspNetCore.Http;

namespace AlltOmHundar.Web.Helpers
{
    public static class SessionHelper
    {
        private const string UserIdKey = "UserId";
        private const string UsernameKey = "Username";
        private const string UserRoleKey = "UserRole";

        public static void SetUser(ISession session, int userId, string username, string role)
        {
            session.SetInt32(UserIdKey, userId);
            session.SetString(UsernameKey, username);
            session.SetString(UserRoleKey, role);
        }

        public static int? GetUserId(ISession session)
        {
            return session.GetInt32(UserIdKey);
        }

        public static string? GetUsername(ISession session)
        {
            return session.GetString(UsernameKey);
        }

        public static string? GetUserRole(ISession session)
        {
            return session.GetString(UserRoleKey);
        }

        public static bool IsLoggedIn(ISession session)
        {
            return session.GetInt32(UserIdKey).HasValue;
        }

        public static bool IsAdmin(ISession session)
        {
            var role = session.GetString(UserRoleKey);
            return role == "Admin";
        }

        public static void ClearUser(ISession session)
        {
            session.Remove(UserIdKey);
            session.Remove(UsernameKey);
            session.Remove(UserRoleKey);
        }
    }
}