#region

using System.Diagnostics;
using System.Text;
using ChaoticOnyx.Hekate.Parser.resources;

#endregion

namespace ChaoticOnyx.Hekate.Parser
{
    public static class CodeIssueExtensions
    {
        /// <summary>
        ///     Получение сообщения для данной проблемы.
        /// </summary>
        /// <param name="issue">Проблема.</param>
        /// <returns>Отформатированное сообщение об проблеме.</returns>
        public static string GetDescription(this CodeIssue issue)
        {
            var result = new StringBuilder();

            var issueId = issue.Id.ToString()
                               .ToUpper();

            var format = ParserResources.ResourceManager.GetString(issueId);
            Debug.Assert(format != null, $"Key {issueId} not localized.");
            result.AppendFormat(format, issue.Arguments);

            return result.ToString();
        }
    }
}
