# Math Parser
This project uses a [Pratt Parser](http://journal.stuffwithstuff.com/2011/03/19/pratt-parsers-expression-parsing-made-easy/) to parse mathematical expresssions in a made-up language. This library takes a string, can convert it into a token stream, parse that stream into a parse tree (returning the root node), and evaluate that into a result. The language has support for multiple types, currently only integers, reals (`double`), lists (`List<double>`), booleans, and strings. This library provides many built-in functions for many mathematical calculations, from square roots to permutations to a help function.

## Usage
In order to use this library, go ahead and download the corresponding Nuget package, which is currently being uploaded this very minute. This library depends on the MathPlus library for some of its functions ([nuget](https://www.nuget.org/packages/MathPlus.Desktop/0.2.0)) ([github](https://github.com/einsteinsci/math-plus)), but it should be downloaded with this package if Nuget is used.

If all you need is something that can parse a string and evaluate the result, `Evaluator.Evaluate()` is the simplist way to do this. To evaluate the string `"5.5 * (34.6 + 2)"`, use this code:
```csharp
double result = Evaluator.Evaluate("5.5 * (34.6 + 2)").ToDouble();
```
The result of `Evaluator.Evaluate()`, or the result from evaluating a parse tree (or any of its nodes), will always be an `IEvaluatable`. This interface defines methods to convert to the C#-friendly types you need for the rest of your code, such as `ToDouble()` above. It also defines the property `CoreValue`, which is a simple object that holds the C#-friendly data without casting it.

## Syntax
The syntax to this language is mostly a blend of C-based languages with mathematical notation.

#### Types
The types used by this library are listed in the enumeration `MathType`. Each of these types has multiple equivalent system types so that excessive casting is avoided. To convert from a `MathType` to a system `Type`, use the static class `MathTypes`. All of these types can be converted to all the others, though some with more difficulty or risk of exceptions than others.

| MathType | IResultValue Type | Stored Internal Type | Valid Convertible System Types |
|:--------:|:-----------------:|:--------------------:| ------------------------------ |
| `Real` | `ResultNumberReal` | `double` | `double`, `float`, `decimal` |
| `Integer` | `ResultNumberInteger` | `long` | `long`, `int`, `short` |
| `String` | `ResultString` | `string` | `string` |
| `Boolean` | `ResultBoolean` | `bool` | `bool` |
| `List` | `ResultList` | `List<double>` | Various* |
\* Specifically, `List<double>`, `List<float>`, `List<decimal>`, `double[]`, `float[]`, and `decimal[]`.

#### Operators
| Operator | Functionality | Valid Types | C# Equivalent |
|:--------:|:-------------:|:-----------:|:-------------:|
| `+` | Addition | Real, Integer | `+` |
| `-` | Subtraction | Real, Integer | `-` |
| `*` | Multiplication* | Real, Integer | `*` |
| `/` | Division | Real, Integer | `/` |
| `%` | Modulus | Real, Integer | `%` |
| `^` | Exponentiation | Real, Integer | `Math.Pow()` |
| `!` | Unary Factorial (Postfix) | Integer | `MathPlus.Probability.Factorial()` |
| `&` | Conditional 'And' | Boolean | `&&` |
| `|` | Conditional 'Or' | Boolean | `||` |
| `~` | Unary 'Not' (Prefix) | Boolean | `!` |
| `=` | Equivalence | (All) | `==` |
| `~=` | Nonequivalence | (All) | `!=` |
| `<` `>` `<=` `>=` | Comparison | Real, Integer | `<` `>` `<=` `>=` |
| `<>` | String Concatenation | String | `System.String.operator+()` |
| `? :` | Conditional Expression** | (All) | `? :` |
\* Implicit mutiplication (like `3a`) is **not** allowed. This is due to the fact that variables (currently only accessible through `VariableRegistry`) can have names longer than one character. If implicit multiplication was allowed, there would be no way to determine if, for example, the input string `"ab"` references a variable called `ab` or two variables `a` and `b` multiplied together implicitly.

\** This uses the same mixfix syntax as in C.

#### Comments and Whitespace
Portions of math can be "commented out" by surrounding them with /* and */, just as in C. Note that there is no "line comment" alternative.

Whitespace only serves the purpose of separating tokens, and is ignored. This is the same as with any C-based language.

#### Lists
List literals use a simplified format based on C-style array literals: `{Expr1, Expr2, Expr3, ...}`. The syntax `{ }` denotes a list with no elements. Note that the contents of a list can include expressions, but if any expression in the list evaluates to something other than a `Real` or `Integer`, an exception will be thrown during the evaluation of the parse tree. Accessing a member from a list is again copied from C, using braces around the element index (zero-based) to access the element. Lists can be used in functions and expressions just as any other type can be.

#### Functions
Functions are called using the same syntax in C: `functionName(arg1, arg2, ...)`. A list of all registered functions can be found by calling the function `helpall()`. It will return a string listing all the functions in a table-like format.

---

## Extensibility
One of the main goals of this project is extensibility. The user of this library should be able to add their own additions to the language for their own use, without having to recompile the original source code (right here). The user can add their own functions very easily by simply applying a few attributes and loading the assembly into the `Extensibility` class. Custom infix, prefix, and suffix operators can be created quite easily by creating several classes, implementing the abstract methods and properties, and applying the necessary atributes. For the more adventurous, custom sytax rules can be created by also implementing the `IInfixParselet` or `IPrefixParselet` interfaces and loading them in your initialization code. For now, only the types mentioned earlier are useable, but custom types may eventually be implemented.



### Examples
All of these examples assume you have loaded the assembly of the classes extending this library during some initialization code. If your extending classes are in the same project that initializes the program, you would load your code like this:
```csharp
// Load current running assembly
Extensibility.LoadedExtensions.Add(Assembly.GetExecutingAssembly());

// It's good to follow this with an initialization call to prevent lag later
Evaluator.Initialize();
```

#### Custom Function
```csharp
// File: TestFunctionLib.cs

// The string in this attribute currently means absolutely nothing.
[FunctionLibrary("testAndStuff")]
public static class TestFunctionLib
{
	// The string entered in this attribute is the name of the function when parsing.
	[MathFunction("plusthree")]
	public static double PlusThree(double val)
	{
		return val + 3
	}
}
```
This allows this expression: `"plusthree(4.2)"` to evaluate to `7.2`.

#### Custom Operator
- Create a class that inherits from `TokenTypeOperator` and implement the abstract property `Operator`. It should be a string that represents the symbol your operator will use.
- Apply a `TokenTypeAttribute` to the class and supply the attribute with a unique string name of the token type for the registry.
- Create a class inheriting from `NodeOperatorBinary` or `NodeOperatorUnary` and implement the abstract methods and properties, and create a constructor that matches the signature of the base class (two `NodeBase`'s for `NodeOperatorBinary`, one for `NodeOperatorUnary`). The abstract property `Operator` is exactly the same as in the token type. 
- Apply a `BinaryOperatorAttribute`, `PrefixOperatorAttribute`, or `PostfixOperatorAttribute` to the original token type class. Supply the same string name given to the `TokenTypeAttribute` along with the typeof the new node class, and the precedence level if the operator is binary. If the operator is right-associative, that is an optional named property for the `BinaryOperatorAttribute`.
- If all your methods and properties are implemented correctly, you will have a new operator to use.

This example operator, using the $ sign, takes a string on the left side and an integer on the right, and returns a string containing the left repeated a number of times equal to the right. In other words, `"Test" $ 3` evaluates to `"TestTestTest"`.

```csharp
// TokenTypeDollar.cs
[BinaryOperator(DOLLAR, typeof(NodeStringRepeat), Precedence.MULTIPLICATIVE)]
[TokenType(DOLLAR)]
public class TokenTypeDollar : TokenTypeOperator
{
	const string DOLLAR = "operatorDollar";

	public static TokenType DollarInstance
	{ get { return TokenTypeRegistry.Get(DOLLAR); } }

	public override string Operator
	{ get { return "$"; } }
}
```
```csharp
// NodeStringRepeat.cs
public class NodeStringRepeat : NodeOperatorBinary
{
	public override TokenType Operator
	{ get { return TokenTypeDollar.DollarInstance; } }

	public override string StringForm
	{ get { return "$"; } }

	public override MathType Type
	{ get { return MathType.String; } }

	public NodeStringRepeat(NodeBase first, NodeBase second) : base(first, second)
	{ }

	public override IResultValue Evaluate()
	{
		string str = First.Evaluate().ToString();
		IResultValue countIRV = Second.Evaluate();
		if (countIRV.Type != MathType.Integer)
		{
			throw new EvaluationException("That's no integer....");
		}
		int count = (int)countIRV.ToInteger();
		string res = "";
		for (int i = 0; i < count; i++)
		{
			res += str;
		}

		return new ResultString(res);
	}
}
```

#### Custom Syntax
Advanced users have the ability to add completely different syntaxes for the grammar of the language, by creating and registering new parselets. Parselets are singleton objects that specify how to parse a token based on its type and turn it into a node. Custom operators simply facilitate this through reflection, but lose the ability to create custom syntax. An infix parselet (such as the function call parslet, triggering off of a `(` token) will be provided the node from parsing the stream up to that point, while a prefix parselet (such as the list literal parselet, triggering off of a `{` token) will not have that information. Due to the logic of a Pratt parser, prefix parselets always have priority over infix parselets, which allows both function calls and parenthesized groups to both trigger off of a `(` token.

To create a parselet, create a class that implements `IPrefixParselet` or `IInfixParselet`. In the `Parse()` method, use the provided Parser parameter to move through the stream and parse tree. Use `Parser.Parse()` to get the next full node available, starting with the next token. Use `Parser.Consume()` to get the next token by itself. Use `Parser.Match()` to check if the next token is of the type you expect. Use these to define your syntax.

To load the parselet, simply subscribe a `PrefixLoadEvent` or `InfixLoadEvent` function to `Parser.PrefixLoading` or to `Parser.InfixLoading` sometime in your initialization logic, before `Evaluator.Initialize()` is called (which should always be called in the initialization logic if custom syntax is being implemented).
