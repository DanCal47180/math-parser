﻿using MathParser.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathParser.ParseTree
{
	public class NodeOperatorNegative : NodeOperatorUnary
	{
		public override string StringForm
		{ get { return "-"; } }

		public override Token Operator
		{ get { return Token.OperatorMinus; } }

		public override MathType Type
		{ get { return MathType.Number; } }

		public NodeOperatorNegative(NodeFactor term)
		{
			Term = term;
		}

		public override ResultValue GetResult()
		{
			return new ResultNumber(-1.0 * Term.GetResult().ToDouble());
		}
	}
}