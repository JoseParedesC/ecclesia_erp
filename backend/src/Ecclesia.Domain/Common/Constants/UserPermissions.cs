namespace Ecclesia.Domain.Common.Constants.Permissions;

public static class EcclesiaPermissions
{
    public static class USER
    {
        public const string READ = "access_manager.user.read";
        public const string CREATE = "access_manager.user.create";
        public const string UPDATE = "access_manager.user.update";
        public const string DELETE = "access_manager.user.delete";
        public const string ASSIGN = "access_manager.user.assign";
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
        public const string READ   = "ecclesia.income.read";
        public const string CREATE = "ecclesia.income.create";
        public const string UPDATE = "ecclesia.income.update";
        public const string DELETE = "ecclesia.income.delete";
    }

    public static class THIRD_PARTIES
    {
        public const string READ   = "ecclesia.third-parties.read";
        public const string CREATE = "ecclesia.third-parties.create";
        public const string UPDATE = "ecclesia.third-parties.update";
        public const string DELETE = "ecclesia.third-parties.delete";
    }

    public static class ACCOUNTING_PERIOD
    {
        public const string READ   = "accounting.accounting-period.read";
        public const string CREATE = "accounting.accounting-period.create";
        public const string REOPEN = "accounting.accounting-period.reopen";
        public const string CLOSE = "accounting.accounting-period.close";
    }

    public static class ACCOUNT
    {
        public const string READ   = "accounting.account.read";
        public const string CREATE = "accounting.account.create";
        public const string UPDATE = "accounting.account.update";
        public const string DELETE = "accounting.account.delete";
    }
}
