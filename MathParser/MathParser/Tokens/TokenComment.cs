﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathParser.Tokens
{
	[MakeToken("comment")]
	public class TokenComment : Token
	{
		public override bool SingleChar
		{ get { return false; } }

		public override int LexerPriority
		{ get { return 1; } }

		public override TokenType Type
		{
			get { return TokenType.Ignored; }
		}

		public override bool Matches(string lexeme)
		{
			return lexeme.StartsWith("/*") && lexeme.EndsWith("*/") &&
				lexeme.Length >= 4; /**/
		}

		public override string ToString()
		{
			return "[Comment]";
		}
	}
}
