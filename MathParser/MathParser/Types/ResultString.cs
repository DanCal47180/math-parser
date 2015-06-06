﻿using MathPlusLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathParser.Types
{
	public sealed class ResultString : IResultValue
	{
		public MathType Type
		{ get { return MathType.String; } }

		public object CoreValue
		{ get { return Value; } }

		public string Value
		{ get; set; }

		public ResultString(string s)
		{
			Value = s;
		}

		public double ToDouble()
		{
			return double.Parse(Value);
		}

		public long ToInteger()
		{
			return long.Parse(Value);
		}

		public bool ToBoolean()
		{
			return ToInteger() == 0 ? false : true;
		}

		public List<double> ToList()
		{
			return new List<double>() { ToDouble() };
		}

		public MathMatrix ToMatrix()
		{
			return new MathMatrix(new double[,] { { ToDouble() } });
		}

		public string ToDisplay()
		{
			return "\"" + Value + "\"";
		}

		public override string ToString()
		{
			return Value;
		}
	}
}
