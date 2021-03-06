﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathParser.Lexing;
using MathParser.ParseTree;
using MathParser.Parsing;
using MathParser.Functions;
using MathParser.Types;
using System.Reflection;

namespace MathParser
{
	/// <summary>
	/// Main class where most parsing is done.
	/// Results of each stage are stored in properties of this class.
	/// </summary>
	public class Evaluator
	{
		/// <summary>
		/// Static instance of singleton
		/// </summary>
		public static Evaluator Instance
		{ get; private set; }

		/// <summary>
		/// Input string from user
		/// </summary>
		public string Input
		{ get; private set; }

		/// <summary>
		/// Token stream used by parser
		/// </summary>
		public TokenStream Lexed
		{ get; private set; }

		/// <summary>
		/// Root node of parse tree, ready to evaluate
		/// </summary>
		public NodeBase ParseTree
		{ get; private set; }

		/// <summary>
		/// Result value in wrapper
		/// </summary>
		public IResultValue Result
		{ get; private set; }

		/// <summary>
		/// Inner result value, ready to be cast
		/// </summary>
		public object ResultValue
		{ get { return Result.CoreValue; } }

		static Evaluator()
		{
			Instance = new Evaluator();
		}

		/// <summary>
		/// Option for on-demand initialization to prevent sudden lag during evaluation.
		/// The calling assembly is loaded via Extensibility unless specified otherwise.
		/// </summary>
		public static void Initialize(bool loadCallingAssembly = true, bool force = false)
		{
			if (loadCallingAssembly)
			{
				Assembly callingAssembly = Assembly.GetCallingAssembly();

				Extensibility.LoadedExtensions.Add(callingAssembly);
			}

			FunctionRegistry.Init(force);
			TokenTypeRegistry.RegisterTokens(force);
			HelpLibrary.Init(force);
			Parser.Init(force);
		}

		#region parts
		/// <summary>
		/// Tokenizes Input into token stream, stored in Lexed
		/// </summary>
		public void Tokenize()
		{
			Lexed = Tokenizer.Tokenize(Input);
		}

		/// <summary>
		/// Tokenizes string into token stream
		/// </summary>
		/// <param name="expression">Expression in string form</param>
		/// <returns>Expression as a token stream</returns>
		public static TokenStream Lex(string expression)
		{
			Instance.Input = expression;
			Instance.Tokenize();
			return Instance.Lexed;
		}

		/// <summary>
		/// Parses Lexed into a parse tree, stored in ParseTree
		/// </summary>
		public void Parse()
		{
			ParseTree = Parser.Parse(Lexed);
		}

		/// <summary>
		/// Parses token stream into a tree
		/// </summary>
		/// <param name="stream">TokenStream to parse</param>
		/// <returns>Parse tree of expression</returns>
		public static NodeBase Parse(TokenStream stream)
		{
			Instance.Lexed = stream;
			Instance.Parse();
			return Instance.ParseTree;
		}

		/// <summary>
		/// Evaluates parse tree. Equivalent to <c>ParseTree.GetResult();</c>
		/// </summary>
		public void Calculate()
		{
			Result = ParseTree.Evaluate();
		}

		/// <summary>
		/// Evaluates parse tree
		/// </summary>
		/// <param name="factor">Parse tree to evaluate</param>
		/// <returns>Evaluated result of parse tree</returns>
		public static IResultValue Calculate(NodeBase factor)
		{
			Instance.ParseTree = factor;
			Instance.Calculate();
			return Instance.Result;
		}
		#endregion

		/// <summary>
		/// Performs all steps of parser at once
		/// </summary>
		public void Evaluate()
		{
			Tokenize();
			Parse();
			Calculate();
		}

		/// <summary>
		/// Performs all steps of parser at once, returning the result
		/// </summary>
		/// <param name="expression">String form of expression</param>
		/// <returns>Result of expression evaluated</returns>
		public static IResultValue Evaluate(string expression)
		{
			Instance.Input = expression;
			Instance.Evaluate();
			return Instance.Result;
		}
	}
}
