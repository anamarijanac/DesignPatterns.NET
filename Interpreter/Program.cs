using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    public class Token
    {
        public enum Type
        {
            Integer, Plus, Minus, LParentheses, RParentheses
        }

        public Type MyType;
        public string Text;

        public Token(Type myType, string text)
        {
            MyType = myType;
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public override string ToString()
        {
            return $"'{Text }'";
        }
    }

    public interface IElement
    {
        int Value{get;}
    }

    public class Integer : IElement
    {
        public int Value
        {
            get;
        }

        public Integer(int value)
        {
            Value = value;
        }
    }

    public class BinaryOperation : IElement
    {
        public enum Type
        {
            Addition, Subtraction
        }

        public Type MyType;
        public IElement Left, Right;

        public int Value
        {
            get
            {
                switch (MyType)
                {
                    case Type.Addition:
                        return Left.Value + Right.Value;
                    case Type.Subtraction:
                        return Left.Value - Right.Value;
                        
                }

                return 0;
            }
        }
    }

    class Program
    {
        static IElement Parse(IReadOnlyList<Token> tokens)
        {
            var result = new BinaryOperation();
            bool haveLeft = false;

            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];
                switch (token.MyType)
                {
                    case Token.Type.Integer:
                        var integer = new Integer(int.Parse(token.Text));
                        if (!haveLeft)
                        {
                            result.Left = integer;
                            haveLeft = true;
                        }
                        else result.Right = integer;
                        break;
                    case Token.Type.Plus:
                        result.MyType = BinaryOperation.Type.Addition;
                        break;
                    case Token.Type.Minus:
                        result.MyType = BinaryOperation.Type.Subtraction;
                        break;
                    case Token.Type.LParentheses:
                        int j = i;
                        for (; j < tokens.Count; j++)
                        {
                            if (tokens[j].MyType == Token.Type.RParentheses)
                                break;
                        }
                        var subexpression = tokens.Skip(i + 1).Take(j - i - 1).ToList();
                        var element = Parse(subexpression);
                        if (!haveLeft)
                        {
                            result.Left = element;
                            haveLeft = true;
                        }
                        else result.Right = element;
                        i = j;
                        break;
                }
            }
            return result;
        }
        static List<Token> Lex(string input)
        {
            List<Token> result = new List<Token>();

            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '+':
                        result.Add(new Token(Token.Type.Plus, "+"));
                        break;
                    case '-':
                        result.Add(new Token(Token.Type.Minus, "-"));
                        break;
                    case '(':
                        result.Add(new Token(Token.Type.LParentheses, "("));
                        break;
                    case ')':
                        result.Add(new Token(Token.Type.RParentheses, ")"));
                        break;
                    default:
                        var sb = new StringBuilder(input[i].ToString());
                        for (int j = i + 1; j < input.Length; j++)
                        {
                            if (char.IsDigit(input[j]))
                            {
                                sb.Append(input[j]);
                                i++;
                            }
                            else
                            {
                                result.Add(new Token(Token.Type.Integer, sb.ToString()));
                                break;
                            }
                        }
                        break;
                }
            }
            return result;
        }

        static void Main(string[] args)
        {
            var input = "(2+13)+(1-8)";
            var tokens = Lex(input);
            Console.WriteLine(string.Join('\t', tokens));

            var parsed = Parse(tokens);

            Console.ReadLine(); 
        }
    }
}
