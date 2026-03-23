namespace Ecclesia.Domain.Common.Constants.Permissions;

public static class EcclesiaPermissions
{
    public static class USER
    {
        public const string READ = "access_manager.user.read";
        public const string CREATE = "access_manager.user.create";
        public const string UPDATE = "access_manager.user.update";
        public const string DELETE = "access_manager.user.delete";
    }

    public static class ROLES
    {
        public const string READ   = "access_manager.roles.read";
        public const string CREATE = "access_manager.roles.create";
        public const string UPDATE = "access_manager.roles.update";
        public const string DELETE = "access_manager.roles.delete";
        public const string ASSIGN = "access_manager.roles.assign";
    }
}
