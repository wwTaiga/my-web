using System;
using System.Collections.Generic;

namespace MyWeb.Constants
{
    public static class Roles
    {
        public const string SUPERADMIN = "SuperAdmin";
        public const string ADMIN = "Admin";
        public const string USER = "User";

        public static IEnumerable<string> GetAllRoles()
        {
            Type type = typeof(Roles);
            foreach (var field in type.GetFields())
            {
                yield return field.GetValue(null) as string;
            }
        }
    }
}
