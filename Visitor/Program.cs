using System;
using System.Collections.Generic;
using System.Text;

namespace Visitor
{
    //"ljepsi nacin" ovo ispisemo da ne bismo dolje morali stalno pisat
    using DictType = Dictionary<Type, Action<Expression, StringBuilder>>;

    #region Bez Vizitora
    public abstract class Expression
    {
        //public abstract void Print(StringBuilder sb);
    }
    #endregion

    //pretpostavimo da nemamo metod print i sad trebamo da ga dodamo svakoj klasi i potklasi
    public class DoubleExpression : Expression
    {
        internal double Value;

        public DoubleExpression(double value)
        {
            Value = value;
        }

        //public override void Print(StringBuilder sb)
        //{
        //    sb.Append(Value);
        //}
    }

    public class AdditionExpression : Expression
    {
        internal Expression Left, RIght;

        public AdditionExpression(Expression left, Expression rIght)
        {
            Left = left ?? throw new ArgumentNullException(nameof(left));
            RIght = rIght ?? throw new ArgumentNullException(nameof(rIght));
        }

        //public override void Print(StringBuilder sb)
        //{
        //    sb.Append("(");
        //    Left.Print(sb);
        //    sb.Append("+");
        //    RIght.Print(sb);
        //    sb.Append(")");
        //}
    }

    public static class ExpressionPrinter
    {
        private static DictType actions = new DictType
        {
            [typeof(DoubleExpression)] = (e, sb) =>
            {
                var de = (DoubleExpression)e;
                sb.Append(de.Value);
            },
            [typeof(AdditionExpression)] = (e, sb) =>
            {
                var ae = (AdditionExpression)e;
                sb.Append("(");
                Print(ae.Left, sb);
                sb.Append("+");
                Print(ae.RIght, sb);
                sb.Append(")");
            }
        };

        public static void Print(Expression e, StringBuilder sb)
        {
            actions[e.GetType()](e, sb);
            //if (e is DoubleExpression de)
            //{
            //    sb.Append(de.Value);
            //}
            //else if (e is AdditionExpression ae)
            //{
            //    sb.Append("(");
            //    Print(ae.Left, sb);
            //    sb.Append("+");
            //    Print(ae.RIght, sb);
            //    sb.Append(")");
            //}
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var sb = new StringBuilder();
            var e = new AdditionExpression(
                new DoubleExpression(2),
                new AdditionExpression(
                    new DoubleExpression(3),
                    new DoubleExpression(1)
                    ));
            ExpressionPrinter.Print(e, sb);
            //e.Print(sb);
            Console.WriteLine(sb.ToString());

            Console.ReadLine();
        }
    }
}
