namespace ChaoticOnyx.Hekate.Parser
{
    /// <summary>
    ///     Виды токенов.
    /// </summary>
    public enum SyntaxKind
    {
        #region Service

        Unknown = 0,
        EndOfLine,
        EndOfFile,

        #endregion

        #region Special

        Identifier,
        WhiteSpace,
        TextLiteral,
        NumericalLiteral,
        PathLiteral,
        Directive,

        #endregion

        #region Comments

        SingleLineComment,
        MultiLineComment,

        #endregion

        #region Keywords

        ForKeyword,
        NewKeyword,
        GlobalKeyword,
        ThrowKeyword,
        CatchKeyword,
        TryKeyword,
        VarKeyword,
        VerbKeyword,
        ProcKeyword,
        InKeyword,
        IfKeyword,
        ElseKeyword,
        SetKeyword,
        AsKeyword,
        WhileKeyword,

        #endregion

        #region Directives

        DefineDirective,
        IncludeDirective,
        IfDefDirective,
        IfNDefDirective,
        EndIfDirective,

        #endregion

        #region Symbols

        Slash,
        BackwardSlashEqual,
        SlashEqual,
        Asterisk,
        AsteriskEqual,
        DoubleAsterisk,
        Equal,
        DoubleEqual,
        ExclamationEqual,
        Exclamation,
        Greater,
        DoubleGreater,
        DoubleGreaterEqual,
        GreaterEqual,
        Lesser,
        DoubleLesser,
        DoubleLesserEqual,
        LesserEqual,
        OpenParenthesis,
        CloseParenthesis,
        OpenBrace,
        CloseBrace,
        OpenBracket,
        CloseBracket,
        Plus,
        PlusEqual,
        DoublePlus,
        Minus,
        MinusEqual,
        DoubleMinus,
        Comma,
        Percent,
        PercentEqual,
        Ampersand,
        DoubleAmpersand,
        AmpersandEqual,
        Colon,
        Question,
        Caret,
        CaretEqual,
        Bar,
        DoubleBar,
        BarEqual,
        Dot,

        #endregion
    }
}
