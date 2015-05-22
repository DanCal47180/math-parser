﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathParser.ParseTree;

namespace MathParser.Tokens
{
	[MakeToken("operatorFactorial", PRIORITY)]
	public class TokenOperatorFactorial : TokenOperator
	{
		public override string Operator
		{ get { return "!"; } }

		public override int Precedence
		{ get { return PREC_PRIMARY; } } // highest precedence

		public override bool SingleChar
		{ get { return true; } }

		public override int ArgumentCount
		{ get { return 1; } }

		public override NodeFactor MakeFactor(NodeFactor[] args)
		{
			return new NodeOperatorFactorial(args[0]);
		}
	}
}
