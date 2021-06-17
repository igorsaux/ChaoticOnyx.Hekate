using System.Text;

namespace ChaoticOnyx.Hekate
{
    /// <summary>
    ///     Проблемы встречаемые в коде.
    /// </summary>
    public record CodeIssue
    {
        public readonly object[]    Arguments;
        public readonly string      Id;
        public readonly SyntaxToken Token;

        /// <summary>
        ///     Создаёт новую запись о проблеме в коде.
        /// </summary>
        /// <param name="id">Идентификатор проблемы.</param>
        /// <param name="token">Токен, с которым связана проблема.</param>
        /// <param name="args">Дополнительные аргументы, используются при выводе сообщения о проблеме.</param>
        public CodeIssue(string id, SyntaxToken token, params object[] args)
        {
            Id        = id;
            Token     = token;
            Arguments = args;
        }

        /// <summary>
        ///     Создаёт и форматирует сообщение об ошибке в формате filePath:line:column [id]: message.
        /// </summary>
        /// <param name="filePath">Путь до файла.</param>
        /// <param name="message">Сообщение для вывода.</param>
        /// <returns>Отформатированное сообщение об ошибке.</returns>
        public string FormatMessage(string filePath, string message)
        {
            var           position = Token.FilePosition;
            StringBuilder sb       = new(message.Length);
            sb.Append($"{filePath}:{position.Line.ToString()}:{position.Column.ToString()} [{Id}]: ");
            sb.AppendFormat(message, Arguments);

            return sb.ToString();
        }
    }
}
