namespace ChaoticOnyx.Hekate.Parser
{
    /// <summary>
    ///     Идентификаторы ошибок.
    /// </summary>
    public enum IssueId
    {
        /// <summary>
        ///     Отсутствует закрывающий символ.
        /// </summary>
        Dm0001 = 0,

        /// <summary>
        ///     Неожиданный токен.
        /// </summary>
        Dm0002,

        /// <summary>
        ///     Неизвестная директива.
        /// </summary>
        Dm0003
    }
}
