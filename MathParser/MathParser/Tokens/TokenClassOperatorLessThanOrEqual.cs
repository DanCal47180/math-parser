﻿using MathParser.ParseTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathParser.Pratt;

namespace MathParser.Tokens
{
	[BinaryOperator("operatorLessThanOrEqual", typeof(NodeOperatorLessThanOrEqual), Precedence.RELATIONAL)]
	[MakeTokenClass("operatorLessThanOrEqual")]
	public class TokenClassOperatorLessThanOrEqual : TokenClassOperator
	{
		public override string Operator
		{ get { return "<="; } }

		public override int PrecedenceLevel
		{ get { return Precedence.RELATIONAL; } }

		public override NodeFactor MakeFactor(NodeFactor[] args)
		{
			return new NodeOperatorLessThanOrEqual(args[0], args[1]);
		}
	}
}
