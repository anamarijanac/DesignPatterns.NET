using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Strategy
{
    public enum OutputFormat
    {
        Markdown,
        Html
    }

    public interface IListStrategy
    {
        void Start(StringBuilder sb);
        void End(StringBuilder sb);
        void AddItem(StringBuilder sb, string item);
    }

    public class MarkdownListStrategy : IListStrategy
    {
        public void AddItem(StringBuilder sb, string item)
        {
            sb.AppendLine($"  * {item}");
        }

        public void End(StringBuilder sb)
        {
            
        }

        public void Start(StringBuilder sb)
        {
            
        }
    }

    public class HtmlListStrategy : IListStrategy
    {
        public void AddItem(StringBuilder sb, string item)
        {
            sb.AppendLine($"  <li>{item}</li>");
        }

        public void End(StringBuilder sb)
        {
            sb.AppendLine("</ul>");
        }

        public void Start(StringBuilder sb)
        {
            sb.AppendLine("<ul>");
        }
    }

    public class TextProcessor//<LS> where LS: IListStrategy, new()
    {
        private StringBuilder sb = new StringBuilder();
        private IListStrategy strategy;// =  new LS();

        public void SetOutputFormat(OutputFormat format)
        {
            switch (format)
            {
                case OutputFormat.Markdown:
                    strategy = new MarkdownListStrategy();
                    break;
                case OutputFormat.Html:
                    strategy = new HtmlListStrategy();
                    break;
                default:
                    break;
            }
        }

        public void AppendList(IEnumerable<string> items)
        {
            strategy.Start(sb);
            foreach (var item in items)
            {
                strategy.AddItem(sb, item);
            }
            strategy.End(sb);
        }

        public void Clear()
        {
            sb.Clear();
        }

        public override string ToString()
        {
            return sb.ToString();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var lista = new string[] { "foo", "bar", "2000" };
            var tp = new TextProcessor();//new TextProcessor<MarkdownListStrategy>();

            tp.SetOutputFormat(OutputFormat.Markdown);
            tp.AppendList(lista);
            Console.WriteLine(tp);

            tp.Clear();

            tp.SetOutputFormat(OutputFormat.Html);
            tp.AppendList(lista);
            Console.WriteLine(tp);

            Console.ReadLine();
        }
    }
}
