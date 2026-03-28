namespace Ecclesia.Domain.Common.Constants.Accounting;

public static class JournalVoucherConstants
{
    public static class Type
    {
        public const string Income    = "INCOME";
        public const string Expense   = "EXPENSE";
        public const string Adjustment = "ADJUSTMENT";
        public const string Reversal  = "REVERSAL";
    }

    public static class Status
    {
        public const string Draft     = "DRAFT";
        public const string Posted    = "POSTED";
        public const string Cancelled = "CANCELLED";
    }
}