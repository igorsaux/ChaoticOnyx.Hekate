using System.Collections.Generic;

namespace ChaoticOnyx.Hekate.Parser
{
	/// <summary>
	///     Минимальная единица компиляции.
	/// </summary>
	public class CompilationUnit
	{
		/// <summary>
		///     Лексер для данной единицы компиляции.
		/// </summary>
		public Lexer Lexer { get; }
		
		public Parser Parser { get; }

		/// <summary>
		///     Создание новой единцы компиляции из текста.
		/// </summary>
		/// <param name="source">Исходный код данной единицы.</param>
		/// <param name="tabWidth">Ширина таба.</param>
		public CompilationUnit(string source, int tabWidth = 4)
		{
			Lexer  = new Lexer(source, tabWidth);
			Parser = new Parser();
		}

		/// <summary>
		///     Создание новой единицы компиляции из набора токенов.
		/// </summary>
		/// <param name="tokens">Набор токенов.</param>
		public CompilationUnit(params SyntaxToken[] tokens) : this(4, tokens) { }

		public CompilationUnit(int tabWidth, params SyntaxToken[] tokens)
		{
			Lexer  = new Lexer(tabWidth, tokens);
			Parser = new Parser();
		}

		/// <summary>
		///     Осуществление парсинга данной единцы компиляции.
		/// </summary>
		public void Parse()
		{
			Lexer.Parse();
			Parser.Parse(Lexer.Tokens);
		}

		public IReadOnlyCollection<CodeIssue> GetIssues()
		{
			List<CodeIssue> issues = new();
			issues.AddRange(Lexer.Issues);
			issues.AddRange(Parser.Issues);

			return issues.AsReadOnly();
		}
		
		/// <summary>
		///     Осуществляет превращение в текст.
		/// </summary>
		public string Emit() { return Lexer.Emit(); }
	}
}
