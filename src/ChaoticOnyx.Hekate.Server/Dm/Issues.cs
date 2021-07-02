using System;

namespace ChaoticOnyx.Hekate.Server.Dm
{
    /// <summary>
    ///     Все возможные ошибки анализаторов.
    ///     Формат - DMSCYXXX.
    ///     Где X - номер уникальный ошибки.
    ///     Y - 1, если это обратное другой ошибке, в остальных случая - 0.
    /// </summary>
    public static class Issues
    {
        /// <summary>
        ///     Возвращает сообщение ошибки в зависимости от его идентификатора.
        /// </summary>
        /// <param name="id">Идентификатор ошибки.</param>
        /// <returns>Сообщение ошибки.</returns>
        public static string IdToMessage(string id)
            => id switch
            {
                // Ошибки анализаторов.
                MissingSpaceAfter  => "Отсутствует пробел после {0}.",
                MissingSpaceBefore => "Отсутствует пробел перед {0}.",
                ExtraSpaceAfter    => "Лишний пробел после {0}.",
                ExtraSpaceBefore   => "Лишний пробел перед {0}.",
                UseSpan            => "Используйте макрос SPAN.",

                // Встроенные ошибки.
                IssuesId.MissingClosingSign => "Отсутствует закрывающий знак {0}.",
                IssuesId.UnexpectedToken    => "Неожиданный токен {0}.",
                IssuesId.ErrorDirective     => "Ошибка: {0}.",
                IssuesId.WarningDirective   => "Предупреждение: {0}.",
                IssuesId.EndIfNotFound      => "#endif для {0} не найден.",
                IssuesId.ExtraEndIf         => "Найден лишний #endif.",
                IssuesId.UnexpectedElse     => "Лишний #else",
                IssuesId.ExpectedProc       => "Ожидалась функция",
                IssuesId.ExpectedValue      => "Ожидалась переменная",
                IssuesId.UnknownVariable    => "Неизвестная переменная препроцессора: {0}",
                IssuesId.InvalidOperator    => "Неизвестный оператор: {0}",
                IssuesId.VariableAlreadyDefined => "Переменная {0} уже была объявлена",
                _                           => throw new NotImplementedException($"Неизвестный идентификатор ошибки: {id}")
            };

        public const string MissingSpaceAfter = "DMSC0001";
        public const string ExtraSpaceAfter   = "DMSC1001";

        public const string MissingSpaceBefore = "DMSC0002";
        public const string ExtraSpaceBefore   = "DMSC1002";

        public const string UseSpan = "DMSC0003";
    }
}
