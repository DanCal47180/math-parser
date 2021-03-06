﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathParser.Lexing;
using MathParser.Types;

namespace MathParser.ParseTree
{
	public class NodeOperatorConcatenate : NodeOperatorBinary
	{
		public override TokenTypeOperator Operator
		{ get { return TokenTypes.OperatorPlus as TokenTypeOperator; } }

		public override MathType Type
		{ get { return MathType.String; } }

		public NodeOperatorConcatenate(NodeBase first, NodeBase second) :
			base(first, second)
		{ }

		public override IResultValue Evaluate()
		{
			IResultValue first = First.Evaluate();
			IResultValue second = Second.Evaluate();

			return new ResultString(First.Evaluate().ToString() + Second.Evaluate().ToString());
		}
	}
}
