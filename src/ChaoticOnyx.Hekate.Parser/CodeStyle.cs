namespace ChaoticOnyx.Hekate.Parser
{
    namespace ChaoticOnyx.Tools.StyleCop
    {
        /// <summary>
        ///     Стиль кода.
        /// </summary>
        public sealed record CodeStyle
        {
            public static readonly CodeStyle Default = new()
            {
                Spaces = new()
                {
                    Catch             = true,
                    For               = true,
                    If                = true,
                    New               = true,
                    Throw             = true,
                    While             = true,
                    Operators         = true,
                    InBrackets        = false,
                    InParentheses     = false,
                    MethodParentheses = false,
                    AfterComma        = true,
                    BeforeComma       = false
                },
                Parentheses =
                {
                    Catch = true,
                    For   = true,
                    If    = true,
                    New   = true,
                    Throw = true,
                    While = true
                },
                Naming =
                {
                    Methods   = NamingConvention.Pascal,
                    Paths     = NamingConvention.Underscored,
                    Variables = NamingConvention.Camel
                },
                LastEmptyLine = true
            };

            public bool LastEmptyLine = true;

            public CodeStyleSpaces Spaces
            {
                get;
                init;
            } = new();

            public CodeStyleParentheses Parentheses
            {
                get;
                init;
            } = new();

            public CodeStyleNaming Naming
            {
                get;
                init;
            } = new();
        }

        /// <summary>
        ///     Стиль наименования.
        /// </summary>
        public enum NamingConvention
        {
            Pascal = 0,
            Camel,
            Underscored
        }

        /// <summary>
        ///     Стиль наименования.
        /// </summary>
        public sealed record CodeStyleNaming
        {
            /// <summary>
            ///     Переменные.
            /// </summary>
            public NamingConvention Variables
            {
                get;
                set;
            } = NamingConvention.Camel;

            /// <summary>
            ///     Проки и вербы.
            /// </summary>
            public NamingConvention Methods
            {
                get;
                set;
            } = NamingConvention.Pascal;

            /// <summary>
            ///     Пути.
            /// </summary>
            public NamingConvention Paths
            {
                get;
                set;
            } = NamingConvention.Underscored;
        }

        /// <summary>
        ///     Скобки
        /// </summary>
        public sealed record CodeStyleParentheses
        {
            /// <summary>
            ///     if (...) / if ...
            /// </summary>
            public bool If
            {
                get;
                set;
            } = true;

            /// <summary>
            ///     while (...) / while ...
            /// </summary>
            public bool While
            {
                get;
                set;
            } = true;

            /// <summary>
            ///     catch (...) / ...
            /// </summary>
            public bool Catch
            {
                get;
                set;
            } = true;

            /// <summary>
            ///     throw (...) / throw ...
            /// </summary>
            public bool Throw
            {
                get;
                set;
            } = true;

            /// <summary>
            ///     for (...) / for ...
            /// </summary>
            public bool For
            {
                get;
                set;
            } = true;

            /// <summary>
            ///     new (...) / new ...
            /// </summary>
            public bool New
            {
                get;
                set;
            } = true;
        }

        /// <summary>
        ///     Пробелы.
        /// </summary>
        public sealed record CodeStyleSpaces
        {
            /// <summary>
            ///     if (...) / if(...)
            /// </summary>
            public bool If
            {
                get;
                set;
            } = true;

            /// <summary>
            ///     while (...) / while(...)
            /// </summary>
            public bool While
            {
                get;
                set;
            } = true;

            /// <summary>
            ///     catch (...) / catch(...)
            /// </summary>
            public bool Catch
            {
                get;
                set;
            } = true;

            /// <summary>
            ///     throw (...) / throw(...)
            /// </summary>
            public bool Throw
            {
                get;
                set;
            } = true;

            /// <summary>
            ///     for (...) / for(...)
            /// </summary>
            public bool For
            {
                get;
                set;
            } = true;

            /// <summary>
            ///     new (...) / new(...)
            /// </summary>
            public bool New
            {
                get;
                set;
            } = true;

            /// <summary>
            ///     ( a + b ) / (a + b)
            /// </summary>
            public bool InParentheses
            {
                get;
                set;
            }

            /// <summary>
            ///     [ a ] / [a]
            /// </summary>
            public bool InBrackets
            {
                get;
                set;
            }

            /// <summary>
            ///     foo () / foo()
            /// </summary>
            public bool MethodParentheses
            {
                get;
                set;
            }

            /// <summary>
            ///     a + b / a+b
            /// </summary>
            public bool Operators
            {
                get;
                set;
            } = true;

            /// <summary>
            ///     a ,b / a,b
            /// </summary>
            public bool BeforeComma
            {
                get;
                set;
            }

            /// <summary>
            ///     a, b / a,b
            /// </summary>
            public bool AfterComma
            {
                get;
                set;
            } = true;
        }
    }
}
