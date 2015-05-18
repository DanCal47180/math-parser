﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathParser;
using MathParser.Tokens;
using MathParser.Lexing;
using MathParser.ParseTree;

namespace MathParser.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.ForegroundColor = ConsoleColor.White;
			Logger.OnLog += Log;
			Logger.DebugLogging = false;

			string input = "1 + 2 * 3 - 4";

			Console.WriteLine("Input> " + input);
			Logger.DebugLogging = true;
			LexStream res = Lexing.Lexer.Lex(input);

			NodeFactor<double> fact = Parser.Parse(res);

			Console.ReadKey(true);
		}

		static void Log(LogLevel level, string message)
		{
			switch (level)
			{
			case LogLevel.Debug:
				Console.ForegroundColor = ConsoleColor.Gray;
				break;
			case LogLevel.Info:
				Console.ForegroundColor = ConsoleColor.White;
				break;
			case LogLevel.Warning:
				Console.ForegroundColor = ConsoleColor.Yellow;
				break;
			case LogLevel.Error:
				Console.ForegroundColor = ConsoleColor.Red;
				break;
			case LogLevel.Fatal:
				Console.ForegroundColor = ConsoleColor.DarkRed;
				break;
			default:
				Console.ForegroundColor = ConsoleColor.Magenta;
				break;
			}

			Console.WriteLine("" + message);

			Console.ForegroundColor = ConsoleColor.White;
		}
	}
}
