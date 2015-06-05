﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathPlusLib;
using System.Reflection;
using MathParser.ParseTree;

namespace MathParser.Functions
{
	[FunctionLibrary("primary")]
	public static class LibraryPrimaryFunctions
	{
		public static List<FunctionInfo> AllFunctions
		{ get; private set; }

		public static bool HasRegistered
		{ get; private set; }

		#region functions

		#region trig
		[Function]
		public static FunctionInfo Sine
		{ get { return sine; } }
		private static FunctionInfo sine = new FunctionInfo(
			(Func<double, double>)MathPlus.Trig.Sin, 
			MathType.Real, "sin", MathType.Real);

		[Function]
		public static FunctionInfo Cosine
		{ get { return cosine; } }
		private static FunctionInfo cosine = new FunctionInfo(
			(Func<double, double>)MathPlus.Trig.Cos, 
			MathType.Real, "cos", MathType.Real);

		[Function]
		public static FunctionInfo Tangent
		{ get { return tangent; } }
		private static FunctionInfo tangent = new FunctionInfo(
			(Func<double, double>)MathPlus.Trig.Tan, 
			MathType.Real, "tan", MathType.Real);

		[Function]
		public static FunctionInfo Cosecant
		{ get { return cosecant; } }
		private static FunctionInfo cosecant = new FunctionInfo(
			(Func<double, double>)MathPlus.Trig.Csc, 
			MathType.Real, "csc", MathType.Real);

		[Function]
		public static FunctionInfo Secant
		{ get { return secant; } }
		private static FunctionInfo secant = new FunctionInfo(
			(Func<double, double>)MathPlus.Trig.Sec, 
			MathType.Real, "sec", MathType.Real);

		[Function]
		public static FunctionInfo Cotangent
		{ get { return cotangent; } }
		private static FunctionInfo cotangent = new FunctionInfo(
			(Func<double, double>)MathPlus.Trig.Cot, 
			MathType.Real, "cot", MathType.Real);

		[Function]
		public static FunctionInfo ArcSine
		{ get { return arcSine; } }
		private static FunctionInfo arcSine = new FunctionInfo(
			(Func<double, double>)MathPlus.Trig.ASin, 
			MathType.Real, "asin", MathType.Real);

		[Function]
		public static FunctionInfo ArcCosine
		{ get { return arcCosine; } }
		private static FunctionInfo arcCosine = new FunctionInfo(
			(Func<double, double>)MathPlus.Trig.ACos, 
			MathType.Real, "acos", MathType.Real);

		[Function]
		public static FunctionInfo ArcTangent
		{ get { return arcTangent; } }
		private static FunctionInfo arcTangent = new FunctionInfo(
			(Func<double, double>)MathPlus.Trig.ATan, 
			MathType.Real, "atan", MathType.Real);
		#endregion

		#region numbers
		[Function]
		public static FunctionInfo LogarithmN
		{ get { return logarithmN; } }
		private static FunctionInfo logarithmN = new FunctionInfo(
			(Func<double, double, double>)MathPlus.Log, 
			MathType.Real, "logn", MathType.Real, MathType.Real);

		[Function]
		public static FunctionInfo LogarithmE
		{ get { return logarithmE; } }
		private static FunctionInfo logarithmE = new FunctionInfo(
			(Func<double, double>)MathPlus.Ln, 
			MathType.Real, "ln", MathType.Real);

		[Function]
		public static FunctionInfo Logarithm
		{ get { return logarithm; } }
		private static FunctionInfo logarithm = new FunctionInfo(
			(Func<double, double>)MathPlus.Log10, 
			MathType.Real, "log", MathType.Real);

		[Function]
		public static FunctionInfo AbsoluteVal
		{ get { return absoluteVal; } }
		private static FunctionInfo absoluteVal = new FunctionInfo(
			(Func<double, double>)MathPlus.Abs, 
			MathType.Real, "abs", MathType.Real);

		[Function]
		public static FunctionInfo Sign
		{ get { return sign; } }
		private static FunctionInfo sign = new FunctionInfo(
			(Func<double, double>)((d) => MathPlus.Sign(d)),
			MathType.Real, "sign", MathType.Real);

		[Function]
		public static FunctionInfo Max
		{ get { return max; } }
		private static FunctionInfo max = new FunctionInfo(
			(Func<double, double, double>)MathPlus.Max,
			MathType.Real, "max", MathType.Real, MathType.Real);

		[Function]
		public static FunctionInfo Min
		{ get { return min; } }
		private static FunctionInfo min = new FunctionInfo(
			(Func<double, double, double>)MathPlus.Min,
			MathType.Real, "min", MathType.Real, MathType.Real);
		#endregion

		[Function]
		public static FunctionInfo Substring
		{ get { return substring; } }
		private static FunctionInfo substring = new FunctionInfo(
			(Func<string, long, long, string>)((string str, long start, long len) => 
				str.Substring((int)start, (int)len)),
			MathType.String, "substring", MathType.String, MathType.Integer, MathType.Integer);

		[Function]
		public static FunctionInfo Help
		{ get { return help; } }
		private static FunctionInfo help = new FunctionInfo(
			(Func<string, string>)HelpLibrary.GetHelp,
			MathType.String, "help", MathType.String);

		[Function]
		public static FunctionInfo GetItem
		{ get { return getItem; } }
		private static FunctionInfo getItem = new FunctionInfo(
			(Func<List<double>, long, double>)((list, i) => list[(int)i]),
			MathType.Real, "get", MathType.List, MathType.Integer);

		#endregion

		public static FunctionInfo GetFunction(string name)
		{
			foreach (FunctionInfo inf in AllFunctions)
			{
				if (inf.Name.ToLower() == name.ToLower())
				{
					return inf;
				}
			}

			return null;
		}

		static LibraryPrimaryFunctions()
		{
			Init();
		}

		public static void Init(bool force = false)
		{
			if (HasRegistered && !force)
			{
				return;
			}

			AllFunctions = new List<FunctionInfo>();
			foreach (Assembly assembly in Extensibility.AllAssemblies)
			{
				Logger.Log(LogLevel.Info, Logger.REGISTRY, 
					"Starting function registry for assembly " + assembly.GetName().Name);

				foreach (Type type in assembly.GetTypes())
				{
					IEnumerable<FunctionLibraryAttribute> libatts =
						type.GetCustomAttributes<FunctionLibraryAttribute>();

					if (libatts == null || libatts.Count() == 0)
					{
						continue;
					}

					PropertyInfo[] properties = type.GetProperties();
					FieldInfo[] fields = type.GetFields();

					foreach (PropertyInfo prop in properties)
					{
						IEnumerable<Attribute> atts = prop.GetCustomAttributes<FunctionAttribute>();
						if (atts == null || atts.Count() == 0)
						{
							continue;
						}
						MethodInfo getter = prop.GetGetMethod(true);
						FunctionInfo func = getter.Invoke(null, new object[0]) as FunctionInfo;

						Logger.Log(LogLevel.Debug, Logger.REGISTRY, "Registering function: " + func.Name);
						AllFunctions.Add(func);
					}

					foreach (FieldInfo f in fields)
					{
						if (!f.IsStatic)
						{
							continue;
						}

						IEnumerable<Attribute> atts = f.GetCustomAttributes<FunctionAttribute>();
						if (atts == null || atts.Count() == 0)
						{
							continue;
						}

						FunctionInfo func = f.GetValue(null) as FunctionInfo;

						Logger.Log(LogLevel.Debug, Logger.REGISTRY, "Registering function: " + func.Name);
						AllFunctions.Add(func);
					}
				}
			}

			HasRegistered = true;
		}
	}
}