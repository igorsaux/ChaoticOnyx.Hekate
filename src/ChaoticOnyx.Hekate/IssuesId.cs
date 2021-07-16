using System;

namespace ChaoticOnyx.Hekate
{
    public static class IssuesId
    {
        public const string MissingClosingSign = "lexer/missing-closing-sign";
        public const string UnexpectedToken    = "lexer/unexpected-token";

        public const string ErrorDirective                = "preprocessor/error-directive";
        public const string WarningDirective              = "preprocessor/warning-directive";
        public const string EndIfNotFound                 = "preprocessor/endif-not-found";
        public const string ExtraEndIf                    = "preprocessor/extra-endif";
        public const string UnexpectedElse                = "preprocessor/unexpected-else";
        public const string ExpectedProc                  = "preprocessor/expecte-proc";
        public const string ExpectedValue                 = "preprocessor/expected-value";
        public const string UnknownVariable               = "preprocessor/unkown-variable";
        public const string InvalidOperator               = "preprocessor/invalid-operator";
        public const string VariableAlreadyDefined        = "preprocessor/variable-already-defined";
        public const string CantCompareNotNumericalValues = "preprocessor/cant-compare-nan-values";

        /// <summary>
        ///     Получает сообщение из идентификатора ошибки.
        /// </summary>
        /// <param name="id">Идентификатор ошибки.</param>
        /// <exception cref="NotImplementedException">Неизвестный идентификатор ошибки.</exception>
        internal static string GetMessage(string id)
            => id switch
            {
                MissingClosingSign            => "Отсутствует закрывающий знак для `{0}`.",
                UnexpectedToken               => "Неожиданный токен `{0}`.",
                ErrorDirective                => "Неизвестная директива `{0}`.",
                WarningDirective              => "Неизвестное определение макроса `{0}`.",
                EndIfNotFound                 => "#endif для `{0}` не найден.",
                ExtraEndIf                    => "Найден лишний #endif.",
                UnexpectedElse                => "Неожиданный #else.",
                ExpectedProc                  => "Ожидалась функция.",
                ExpectedValue                 => "Ожидалось значение.",
                UnknownVariable               => "Неизвестная переменная.",
                InvalidOperator               => "Неизвестный оператор.",
                VariableAlreadyDefined        => "Переменная `{0}` уже была объявлена.",
                CantCompareNotNumericalValues => "Нельзя сравнить не числовые значения.",
                _                             => throw new NotImplementedException($"Неизвестный идентификатор ошибки: {id}")
            };
    }
}
