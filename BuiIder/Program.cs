using System;
using System.Collections.Generic;
using System.Text;

namespace BuiIder
{
    #region Builder

    public class HtmlElement
    {
        public String Name, Text;
        public List<HtmlElement> Elements = new List<HtmlElement>();
        private const int indentsize = 2;

        public HtmlElement(string name, string text)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public HtmlElement()
        {

        }

        public string ToStringImpl(int indent)
        {
            var sb = new StringBuilder();
            var i = new string(' ', indentsize*indent);

            sb.AppendLine($"{i}<{Name}>");
            if (!string.IsNullOrEmpty(Text))
            {
                sb.Append(new string(' ', indentsize * (indent + 1)));
                sb.AppendLine(Text);
            }

            foreach (var element in Elements)
            {
                sb.Append(element.ToStringImpl(indent + 1));//rekurzija
            }

            sb.AppendLine($"{i}</{Name}>");

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToStringImpl(0);
        }
    }

    public class HtmlBuilder
    {
        private readonly string rootName;
        HtmlElement root = new HtmlElement();

        public HtmlBuilder(string rootName)
        {
            this.rootName = rootName ?? throw new ArgumentNullException(nameof(rootName));
            root.Name = rootName;
        }
        //fluent builder - umjesto void vraca HtmlBuilder, pa možemo da nizemo naredbe
        public HtmlBuilder AddChild(string childName, string childText)
        {
            var e = new HtmlElement(childName, childText);
            root.Elements.Add(e);
            return this;

        }

        public void Clear()
        {
            root = new HtmlElement { Name = rootName };
        }

        public override string ToString()
        {
            return root.ToString();
        }
    }
    #endregion
    class Program
    {
        static void Main(string[] args)
        {
            #region Bez buildera

            //ovako bi pravili da nema paterna
            var hello = "Hello";

            var sb = new StringBuilder();

            sb.Append("<p>");
            sb.Append(hello);
            sb.Append("</p>");
            Console.WriteLine(sb);

            var words = new[] { "Hello", "World!" };

            sb.Clear();
            sb.Append("<ul>");
            foreach (var word in words)
            {
                sb.Append($"<li>{word}</li>");
            }
            sb.Append("</ul>");
            Console.WriteLine(sb);
            // sve rucno

            #endregion

            #region Sa Bilderom
            var b = new HtmlBuilder("ul");
            b.AddChild("li", "Hello");
            b.AddChild("li", "World!");

            Console.Write(b.ToString());
            #endregion

            #region Sa Fluent Bilderom
            var bl = new HtmlBuilder("ul");
            bl.AddChild("li", "Hello").AddChild("li", "World!");

            Console.Write(b.ToString());
            #endregion


            Console.ReadLine();
        }
    }
}
