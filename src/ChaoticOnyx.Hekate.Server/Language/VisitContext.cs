using System.Collections.Generic;

namespace ChaoticOnyx.Hekate.Server.Language
{
    public record VisitContext(LinkedList<SyntaxToken> Tokens, LinkedListNode<SyntaxToken> Token, PreprocessorContext PreprocessorContext);
}
