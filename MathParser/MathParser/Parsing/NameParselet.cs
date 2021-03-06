﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathParser.ParseTree;
using MathParser.Lexing;

namespace MathParser.Parsing
{
	public class NameParselet : IPrefixParselet
	{
		public NodeBase Parse(Parser parser, Token token)
		{
			return new NodeIdentifier(token.Lexeme);
		}
	}
}
