using System;
/*
 * Dodajemo funkcionalnosti (metode i propertije) tako sto napravimo novu klasu koja u sebi ima kao properti staru
 * tako mozemo koristiti i sve metode i propertije stare klase a i nove 
 */
    
namespace DynamicDecoratorComposition
{
    //jedan interfejs da vlada svima >:D    da bi circle, square i sve ostale zbuvljali po isto ime
    public interface IShape
    {
        string AsString();
    }

    public class Circle : IShape
    {
        private float radius;

        public Circle(float radius)
        {
            this.radius = radius;
        }

        public string AsString()
        {
            return $"Circle with radius {radius}";
        }

        public void Resize(float factor)
        {
            radius *= factor;
        }
    }

    public class Square : IShape
    {
        private float side;

        public Square(float side)
        {
            this.side = side;
        }

        public string AsString()
        {
            return $"Square with side {side}";
        }
    }

    //dekorator i njega zbuvljamo pod IShape da bi ga mogli dalje dekorisati
    public class ColoredShape : IShape
    {
        //delegat - on nam je veza sa nekim od IShapeova
        private IShape shape;
        private string color;

        public ColoredShape(IShape shape, string color)
        {
            this.shape = shape ?? throw new ArgumentNullException(nameof(shape));
            this.color = color ?? throw new ArgumentNullException(nameof(color));
        }

        public string AsString()
        {
            return $"{shape.AsString()} has the {color} color";
        }
    }
    //drugi dekorator i on je otvoren za dalje dekorisanje
    public class TransparentShape : IShape
    {
        private IShape shape;
        private float transparency;

        public TransparentShape(IShape shape, float transparency)
        {
            this.shape = shape ?? throw new ArgumentNullException(nameof(shape));
            this.transparency = transparency;
        }

        public string AsString()
        {
            return $"{shape.AsString()} has {transparency}% transparency";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //moze svakako
            var square = new Square(18.27f);
            Console.WriteLine(square.AsString());

            var coloredSquare = new ColoredShape(square, "red");
            Console.WriteLine(coloredSquare.AsString());

            var transparentColoredShape = new TransparentShape(coloredSquare, 50);
            Console.WriteLine(transparentColoredShape.AsString());

            var transparentShape = new TransparentShape(square, 17);
            Console.WriteLine(transparentShape.AsString());

            var transparentColoredCircle = new TransparentShape(new ColoredShape(new Circle(5.2f), "blue"), 2);
            Console.WriteLine(transparentColoredCircle.AsString());

            var coloredTransparentCircle = new ColoredShape(new TransparentShape(new Circle(2.0f), 9), "yellow");
            Console.WriteLine(coloredTransparentCircle.AsString());

            Console.ReadLine();
        }
    }
}
