namespace Ecclesia.Domain.Common.Constants;

public static class RolePermissionConstants
{
    public static class Schema
    {
        public const string AccessManager = "access_manager";
        public const string Ecclesia      = "ecclesia";
    }

    public static class Option
    {
        public const string User            = "user";
        public const string Roles           = "roles";
        public const string RolePermissions = "role_permissions";
    }

    public static class Permission
    {
        public const string Read   = "read";
        public const string Create = "create";
        public const string Update = "update";
        public const string Delete = "delete";
        public const string Assign = "assign";
    }
    
}