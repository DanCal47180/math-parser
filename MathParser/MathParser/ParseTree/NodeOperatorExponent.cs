﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathParser.Tokens;
using MathPlusLib;
using MathParser.Types;

namespace MathParser.ParseTree
{
	public class NodeOperatorExponent : NodeOperatorBinary
	{
		public override TokenType Operator
		{ get { return TokenType.OperatorExponent; } }

		public override MathType Type
		{ get { return MathType.Real; } }

		public override string StringForm
		{ get { return "^"; } }

		public NodeOperatorExponent(NodeFactor first, NodeFactor second) :
			base(first, second)
		{ }

		public override IResultValue GetResult()
		{
			return new ResultNumberReal(MathPlus.Pow(First.GetResult().ToDouble(), Second.GetResult().ToDouble()));
		}
	}
}
