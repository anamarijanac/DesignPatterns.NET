using System;

namespace FactoryMethod
{
    public class Point
    {
        public double x;
        public double y;

        private Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        //problem, ne mozemo 2 ista konstruktora

        #region hocu polarne koordinate a ne merem

        //public Point(double rho, double theta)
        //{
        //    this.x = rho;
        //    this.y = theta;
        //}
        #endregion

        //jedan od nacina da rijesimo ovo je factory method
        //konstruktor postaje private i napravimo metode koji mu pristupaju i salju mu ispravne podatke

        public static Point NewCarthesianPoint(double x, double y)
        {
            return new Point(x, y);
        }

        public static Point NewPolarPoint(double rho, double theta)
        {
            return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
        }

        //Factory nastaje ako uzmemo ova dva metoda i prebacimo ih u klasu koja ce samo njih imati
        //onda imamo problem private konstruktora, možemo ga ili staviti internal ili ugnijezditi factory u klasu


    }

    class Program
    {
        static void Main(string[] args)
        {
            var p = Point.NewCarthesianPoint(2, 3);
            var pp = Point.NewPolarPoint(1,  Math.PI / 2);
            Console.ReadLine();
        }
    }
}
