namespace ChaoticOnyx.Hekate
{
    public static class IssuesId
    {
        // 0*** - Ошибики лексера.
        // 1*** - Ошибки препроцессора.
        public const string MissingClosingSign = "DM0001";
        public const string UnexpectedToken    = "DM0002";

        public const string ErrorDirective         = "DM1000";
        public const string WarningDirective       = "DM1001";
        public const string EndIfNotFound          = "DM1002";
        public const string ExtraEndIf             = "DM1003";
        public const string UnexpectedElse         = "DM1004";
        public const string ExpectedProc           = "DM1005";
        public const string ExpectedValue          = "DM1006";
        public const string UnknownVariable        = "DM1007";
        public const string InvalidOperator        = "DM1008";
        public const string VariableAlreadyDefined = "DM1009";
    }
}
