using System;

namespace LiskovSubstitutionPrinciple
{
    public class Rectangle
    {
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }

        public Rectangle()
        {

        }

        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"Rectangle: {nameof(Width)} : {Width}, {nameof(Height)} : {Height}";
        }
    }

    public class Square : Rectangle
    {

        public override int Width
        {
            set { base.Width = base.Height = value; }
        }

        public override int Height
        {
            set { base.Width = base.Height = value; }
        }
    }

    class Program
    {
        public static int Area(Rectangle r) => r.Height * r.Width;

        static void Main(string[] args)
        {
            var rc = new Rectangle(2, 3); 
            Console.WriteLine($"{rc}, Area : {Area(rc)}");

            Rectangle sq = new Square();
            sq.Width = 4;

            Console.WriteLine($"{sq}, Area : {Area(sq)}");

            Console.ReadLine();
        }
    }
}
