using System.Collections.Generic;
using System.Linq;

namespace ChaoticOnyx.Hekate.Parser
{
	public sealed record PreprocessorWithTokens(IList<SyntaxToken> Tokens);

	public sealed class Preprocessor
	{
		private readonly List<SyntaxToken>          _defines  = new();
		private readonly Stack<SyntaxToken>         _ifs      = new();
		private readonly List<SyntaxToken>          _includes = new();
		private readonly List<CodeIssue>            _issues   = new();
		private readonly TypeContainer<SyntaxToken> _tokens;

		public IReadOnlyCollection<CodeIssue> Issues   => _issues.AsReadOnly();
		public IList<SyntaxToken>             Includes => _includes;
		public IList<SyntaxToken>             Defines  => _defines;

		private Preprocessor(IList<SyntaxToken>? tokens = null) { _tokens = new TypeContainer<SyntaxToken>(tokens ?? new List<SyntaxToken>()); }

		public static Preprocessor WithTokens(IList<SyntaxToken> tokens) { return new(tokens); }

		public static Preprocessor WithoutTokens() { return new(); }

		/// <summary>
		///     Производит препроцессинг токенов.
		/// </summary>
		public void Preprocess()
		{
			_ifs.Clear();
			_defines.Clear();
			_includes.Clear();
			_issues.Clear();

			while (!_tokens.IsEnd)
			{
				SyntaxToken  token = _tokens.Read();
				SyntaxToken? next  = _tokens.Peek();

				if (next is null) { break; }

				switch (token.Kind)
				{
					case SyntaxKind.DefineDirective:
						_defines.Add(next);

						break;
					case SyntaxKind.IncludeDirective:
						_includes.Add(next);

						break;
					case SyntaxKind.IfDefDirective:
						_ifs.Push(token);

						if (_defines.Any(t => t.Text == next.Text)) { break; }

						SkipIf();

						break;
					case SyntaxKind.IfNDefDirective:
						_ifs.Push(token);

						if (_defines.Count == 0 || _defines.Any(t => t.Text != next.Text)) { break; }

						SkipIf();

						break;
					case SyntaxKind.UndefDirective:
						SyntaxToken? define = _defines.Find(t => t.Text == next.Text);

						if (define != null) { _defines.Remove(define); }

						_issues.Add(new CodeIssue(IssuesId.UnknownMacrosDefinition, next, next.Text));

						break;
					case SyntaxKind.EndIfDirective:
						if (_ifs.Count == 0)
						{
							_issues.Add(new CodeIssue(IssuesId.ExtraEndIf, token));

							break;
						}

						_ifs.Pop();

						break;
				}
			}

			if (_ifs.Count > 0)
			{
				SyntaxToken last = _ifs.Last();
				_issues.Add(new CodeIssue(IssuesId.EndIfNotFound, last, last.Text));
			}
		}

		/// <summary>
		///     Пропускает все токены до первого нахождение #endif.
		/// </summary>
		/// <returns>Возвращает true - если #endif был найден.</returns>
		private void SkipIf()
		{
			while (!_tokens.IsEnd)
			{
				SyntaxToken? token = _tokens.Peek();

				if (token == null) { return; }

				if (token.Kind == SyntaxKind.EndIfDirective) { return; }

				_tokens.Advance();
			}
		}
	}
}
