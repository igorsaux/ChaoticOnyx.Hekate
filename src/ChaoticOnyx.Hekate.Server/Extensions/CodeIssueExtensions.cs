using System.Collections.Generic;
using System.Linq;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace ChaoticOnyx.Hekate.Server.Extensions
{
    public static class CodeIssueExtensions
    {
        public static Diagnostic ToDiagnostic(this CodeIssue issue)
        {
            FileLine position      = issue.Token.FilePosition;
            Position startPosition = new Position(position.Line, position.Column);
            Position endPosition   = new Position(position.Line, position.Column + issue.Token.Length);

            return new Diagnostic
            {
                Code     = issue.Id,
                Message  = string.Format(issue.Message, issue.MessageArguments),
                Range    = new Range(startPosition, endPosition),
                Severity = (DiagnosticSeverity)issue.Severity,
                Source   = "Hekate"
            };
        }

        public static Container<Diagnostic> ToDiagnostics(this IEnumerable<CodeIssue> issues)
        {
            return Container.From(issues.Select(i => i.ToDiagnostic()));
        }
    }
}
