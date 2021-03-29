#region

using System.Diagnostics;
using System.Resources;
using System.Text;

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
        public static string GetDescription(this CodeIssue issue, ResourceManager resources)
        {
            var result  = new StringBuilder();
            var issueId = issue.Id.ToUpper();
            var format  = resources.GetString(issueId);
            Debug.Assert(format != null, $"Key {issueId} not localized.");
            result.AppendFormat(format, issue.Arguments);

            return result.ToString();
        }
    }
}
