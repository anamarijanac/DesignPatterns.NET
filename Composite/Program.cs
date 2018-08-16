using System;
using System.Collections.Generic;
using System.Text;

namespace Composite
{
    public class GraphicObject
    {
        //posto je virtual mozemo ga overrajdovati u klasama koje nasledjuju ovu
        public virtual string Name { get; set; } = "Group";
        public string Color;

        //djeca su opciona, ovako ih nece loadovat dok ih ne trazimo
        private Lazy<List<GraphicObject>> children = new Lazy<List<GraphicObject>>();
        public List<GraphicObject> Children => children.Value;

        public void Print(StringBuilder sb, int depth)
        {
            sb.Append(new string('*', depth))
                .Append(string.IsNullOrWhiteSpace(Color) ? string.Empty : $"{Color} ")
                .AppendLine(Name);
            foreach (var child in Children)
            {
                child.Print(sb, depth + 1);
            }

            
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            Print(sb, 0);
            return sb.ToString();
        }
    }

    public class Circle : GraphicObject
    {
        public override string Name => "Circle";
    }

    public class Square : GraphicObject
    {
        public override string Name => "Square";
    }

    class Program
    {
        static void Main(string[] args)
        {
            var drawing = new GraphicObject { Name = "My drawing" };
            drawing.Children.Add(new Circle { Color = "Red" });
            drawing.Children.Add(new Square { Color = "Green" });

            var group = new GraphicObject();
            group.Children.Add(new Circle { Color = "Rouge" });
            group.Children.Add(new Square { Color = "Verte" });

            drawing.Children.Add(group);

            Console.WriteLine(drawing);


            Console.ReadLine();
        }
    }
}
