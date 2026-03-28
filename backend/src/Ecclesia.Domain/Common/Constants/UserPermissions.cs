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

    public static class INCOME
    {
        public const string READ   = "access_manager.income.read";
        public const string CREATE = "access_manager.income.create";
        public const string UPDATE = "access_manager.income.update";
        public const string DELETE = "access_manager.income.delete";
    }

    public static class THIRD_PARTIES
    {
        public const string READ   = "access_manager.third-parties.read";
        public const string CREATE = "access_manager.third-parties.create";
        public const string UPDATE = "access_manager.third-parties.update";
        public const string DELETE = "access_manager.third-parties.delete";
    }
}
