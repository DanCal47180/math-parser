﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathParser.Tokens
{
	[Token("operatorMultiply", PRIORITY)]
	public class TokenOperatorMultiply : TokenOperator
	{
		public override string Operator
		{ get { return "*"; } }

		public override int Precedence
		{ get { return PREC_MULTIPLICATIVE; } }

		public override bool SingleChar
		{ get { return true; } }
	}
}
