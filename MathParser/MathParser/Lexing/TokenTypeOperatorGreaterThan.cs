﻿using MathParser.ParseTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathParser.Parsing;

namespace MathParser.Lexing
{
	[BinaryOperator("operatorGreaterThan", typeof(NodeOperatorGreaterThan), Precedence.RELATIONAL)]
	[TokenType("operatorGreaterThan")]
	public class TokenTypeOperatorGreaterThan : TokenTypeOperator
	{
		public override string StringForm
		{ get { return ">"; } }
	}
}
