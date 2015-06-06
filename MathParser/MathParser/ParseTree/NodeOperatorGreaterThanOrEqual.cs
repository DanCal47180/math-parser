﻿using MathParser.Lexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathParser.Types;

namespace MathParser.ParseTree
{
	public class NodeOperatorGreaterThanOrEqual : NodeOperatorBinary
	{
		public override string StringForm
		{ get { return ">="; } }

		public override TokenType Operator
		{ get { return TokenType.OperatorGreaterThanOrEqual; } }

		public override MathType Type
		{ get { return MathType.Boolean; } }

		public NodeOperatorGreaterThanOrEqual(NodeFactor first, NodeFactor second) :
			base(first, second)
		{ }

		public override IResultValue GetResult()
		{
			return new ResultBoolean(First.GetResult().ToDouble() >= Second.GetResult().ToDouble());
		}
	}
}
