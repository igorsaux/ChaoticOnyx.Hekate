namespace ChaoticOnyx.Hekate
{
    public static class IssuesId
    {
        // 0*** - Ошибики лексера.
        // 1*** - Ошибки препроцессора.
        public const string MissingClosingSign = "DM0001";
        public const string UnexpectedToken    = "DM0002";
        public const string UnknownDirective   = "DM0003";

        public const string ErrorDirective   = "DM1000";
        public const string WarningDirective = "DM1001";
        public const string EndIfNotFound    = "DM1002";
        public const string ExtraEndIf       = "DM1003";
        public const string UnexpectedElse   = "DM1004";
    }
}
