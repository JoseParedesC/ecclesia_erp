using System.Reflection;

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


    public static class ROSTRO
    {
        public const string Read       = "ecclesia.rostro.read";
        public const string Create     = "ecclesia.rostro.create";
        public const string Update     = "ecclesia.rostro.update";
        public const string Deactivate = "ecclesia.rostro.deactivate";
    }

    public static Dictionary<string, Dictionary<string, List<string>>> ToDictionary()
    {
        var result = new Dictionary<string, Dictionary<string, List<string>>>();

        var nestedTypes = typeof(EcclesiaPermissions).GetNestedTypes(BindingFlags.Public);

        foreach (var type in nestedTypes)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            foreach (var field in fields)
            {
                if (field.FieldType != typeof(string)) continue;

                var value = field.GetValue(null)?.ToString();
                if (string.IsNullOrWhiteSpace(value)) continue;

                // access_manager.user.read
                var parts = value.Split('.');
                if (parts.Length != 3) continue;

                var schema = parts[0];
                var module = parts[1];
                var action = parts[2];

                if (!result.ContainsKey(schema))
                    result[schema] = new Dictionary<string, List<string>>();

                if (!result[schema].ContainsKey(module))
                    result[schema][module] = new List<string>();

                result[schema][module].Add(action);
            }
        }

        return result;
    }

}
