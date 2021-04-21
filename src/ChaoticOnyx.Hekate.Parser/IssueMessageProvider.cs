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
        /// <param name="resources">Менеджер ресурсов, из которого будет браться локализованная строка.</param>
        /// <returns>Отформатированное сообщение об проблеме.</returns>
        public static string GetDescription(this CodeIssue issue, ResourceManager resources)
        {
            StringBuilder? result  = new();
            string         issueId = issue.Id.ToUpper();
            string?        format  = resources.GetString(issueId);
            Debug.Assert(format != null, $"Key {issueId} not localized.");

            if (issue.Arguments.Length == 0)
            {
                result.Append(format);
            }
            else
            {
                result.AppendFormat(format, issue.Arguments);
            }

            return result.ToString();
        }
    }
}
