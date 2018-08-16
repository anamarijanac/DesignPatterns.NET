using System;

namespace StaticDecoratorComposition
{
    public abstract class Shape
    {

        public abstract string AsString();
    }

    public class Circle : Shape
    {
        private float radius;

        public Circle()
        {

        }

        public Circle(float radius)
        {
            this.radius = radius;
        }

        public override string AsString()
        {
            return $"Circle with radius {radius}";
        }

        public void Resize(float factor)
        {
            radius *= factor;
        }
    }

    public class Square : Shape
    {
        private float side;

        public Square()
        {

        }

        public Square(float side)
        {
            this.side = side;
        }

        public override string AsString()
        {
            return $"Square with side {side}";
        }
    }

    public class ColoredShape : Shape
    {
        private Shape shape;
        private string color;

        public ColoredShape(Shape shape, string color)
        {
            this.shape = shape ?? throw new ArgumentNullException(nameof(shape));
            this.color = color ?? throw new ArgumentNullException(nameof(color));
        }

        public override string AsString()
        {
            return $"{shape.AsString()} has the {color} color";
        }
    }

    public class TransparentShape : Shape
    {
        private Shape shape;
        private float transparency;

        public TransparentShape(Shape shape, float transparency)
        {
            this.shape = shape ?? throw new ArgumentNullException(nameof(shape));
            this.transparency = transparency;
        }

        public override string AsString()
        {
            return $"{shape.AsString()} has {transparency}% transparency";
        }
    }

    public class ColoredShape<T> : Shape where T : Shape, new()
    {
        private T shape = new T();
        private string color;
        private string tree;

        public ColoredShape(string color)
        {
            this.color = color ?? throw new ArgumentNullException(nameof(color));
        }

        public ColoredShape() : this("Ultraviolet")
        {

        }

        public override string AsString()
        {
            return $"{shape.AsString()} has the {color} color";
        }
    }

    public class TransparentShape<T> : Shape where T : Shape, new()
    {
        T shape = new T();
        private float transparency;

        public TransparentShape(float transparency)
        {
            this.transparency = transparency;
        }

        public TransparentShape() : this(20)
        {

        }

        public override string AsString()
        {
            return $"{shape.AsString()} has {transparency}% transparency";
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var tcs = new TransparentShape<ColoredShape<Square>>();
            var cts = new ColoredShape<TransparentShape<Square>>();
            Console.WriteLine(tcs.AsString());
            Console.WriteLine(cts.AsString());

            Console.ReadLine();
        }
    }
}