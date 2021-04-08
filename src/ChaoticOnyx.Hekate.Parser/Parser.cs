using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ChaoticOnyx.Hekate.Parser.SyntaxNodes;

namespace ChaoticOnyx.Hekate.Parser
{
	public sealed class Parser
	{
		private readonly List<CodeIssue>             _issues = new();
		private          TypeContainer<SyntaxToken>? _tokens;

		public CompilationUnitNode? Root { get; private set; }

		public IReadOnlyCollection<CodeIssue> Issues => _issues.AsReadOnly();

		/// <summary>
		///     Производит парсинг токенов.
		/// </summary>
		/// <param name="tokens">Токены.</param>
		/// <returns>Синтаксическое дерево.</returns>
		public void Parse(IList<SyntaxToken> tokens)
		{
			_tokens = new TypeContainer<SyntaxToken>(tokens);
			Root    = new CompilationUnitNode();
			ParseBody();
		}

		private void ParseDeclaration()
		{
			_ = _tokens ?? throw new InvalidOperationException($"{nameof(_tokens)} is null.");
			_tokens.Start();
			List<SyntaxToken> path            = new();
			var               declarationInfo = new DeclarationParsingInfo { Kind = NodeKind.TypeDeclaration };

			while (!_tokens.IsEnd)
			{
				SyntaxToken? token = _tokens.Peek();
				SyntaxToken? next  = _tokens.Peek(2);

				if (token is null) { return; }

				bool isBegin = path.All(t => t.Kind != SyntaxKind.Identifier);

				switch (token.Kind)
				{
					case SyntaxKind.Slash:
						if (path.LastOrDefault()
								?.Kind == SyntaxKind.Slash)
						{
							_issues.Add(new CodeIssue(IssuesId.UnexpectedToken, token));
						}

						path.Add(token);

						break;
					case SyntaxKind.Identifier:
						declarationInfo.Head = token;
						path.Add(token);

						break;
					case SyntaxKind.ProcKeyword:
						if (isBegin)
						{
							declarationInfo.Kind = NodeKind.ProcDeclaration;
							path.Add(token);
						}
						else
						{
							CreateDeclaration(declarationInfo, path.ToImmutableList());
							declarationInfo.Kind = NodeKind.ProcDeclaration;
							path.Add(token);
						}

						break;
					case SyntaxKind.VerbKeyword:
						if (isBegin)
						{
							declarationInfo.Kind = NodeKind.VerbDeclaration;
							path.Add(token);
						}
						else
						{
							CreateDeclaration(declarationInfo, path.ToImmutableList());
							declarationInfo.Kind = NodeKind.VerbDeclaration;
							path.Add(token);
						}

						break;
					case SyntaxKind.VarKeyword:
						if (isBegin)
						{
							declarationInfo.Kind = NodeKind.VariableDeclaration;
							path.Add(token);
						}
						else
						{
							CreateDeclaration(declarationInfo, path.ToImmutableList());
							declarationInfo.Kind = NodeKind.VariableDeclaration;
							path.Add(token);
						}

						break;
					case SyntaxKind.EndOfFile:
						if (path.Count == 0) { return; }

						CreateDeclaration(declarationInfo, path.ToImmutableList());

						break;
					default:
						_issues.Add(new CodeIssue(IssuesId.UnexpectedToken, token));

						return;
				}

				_tokens.Advance();
			}
		}

		private void ParseBody()
		{
			_ = _tokens ?? throw new InvalidOperationException($"{nameof(_tokens)} is null.");

			while (!_tokens.IsEnd)
			{
				_tokens.Start();
				SyntaxToken? token = _tokens.Peek();
				SyntaxToken? next  = _tokens.Peek(2);

				if (token is null) { return; }

				switch (token.Kind)
				{
					case SyntaxKind.Slash:
						if (next is
						{
							Kind: SyntaxKind.Identifier or SyntaxKind.VerbKeyword or SyntaxKind.ProcKeyword or
							SyntaxKind.VarKeyword
						}) { ParseDeclaration(); }

						break;
					case SyntaxKind.VarKeyword:
					case SyntaxKind.ProcKeyword:
					case SyntaxKind.VerbKeyword:
					case SyntaxKind.Identifier:
						ParseDeclaration();

						break;
					case SyntaxKind.EndOfFile:
						return;
				}
				
				_tokens.Advance();
			}
		}

		private void CreateDeclaration(DeclarationParsingInfo info, IList<SyntaxToken> path)
		{
			_ = info.Head ?? throw new InvalidOperationException($"{nameof(info.Head)} is null.");

			switch (info.Kind)
			{
				case NodeKind.TypeDeclaration:
					AddDeclaration(new TypeDeclarationNode(info.Head, path));

					break;
				case NodeKind.VariableDeclaration:
					AddDeclaration(new VariableDeclarationNode(info.Head, path));

					break;
				case NodeKind.VerbDeclaration:
					AddDeclaration(new VerbDeclarationNode(info.Head, path));

					break;
				case NodeKind.ProcDeclaration:
					AddDeclaration(new ProcDeclarationNode(info.Head, path));

					break;
				default:
					throw new InvalidOperationException($"{info.Kind} is not declaration kind.");
			}
		}

		private void AddDeclaration(DeclarationNode declaration)
		{
			_ = Root ?? throw new InvalidOperationException($"{nameof(Root)} is null");
			Root.Declarations.Add(declaration);
		}

		private sealed class DeclarationParsingInfo
		{
			public SyntaxToken? Head;
			public NodeKind     Kind;

			public DeclarationParsingInfo() { Kind = NodeKind.Declaration; }
		}
	}
}
