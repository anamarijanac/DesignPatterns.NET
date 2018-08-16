using System;

namespace MultipleInheritance
{
    public class Bird : IBird
    {
        public void Fly()
        {
            Console.WriteLine("flying");
        }
    }

    public class Lizzard : ILizzard
    {
        public void Crawl()
        {
            Console.WriteLine("crawling");
        }
    }

    public class Dragon : IBird, ILizzard      //: Bird,Lizzard
    {
        private Lizzard lizzard;
        private Bird bird;

        public Dragon()
        {
            this.lizzard = new Lizzard();
            this.bird = new Bird();
        }

        //ne zelimo da predefinisemo interfejse, zelimo da se fly() i crawl() rade ptica i guster koje je progutao zmaj 
        public void Crawl()
        {
            lizzard.Crawl();
        }

        public void Fly()
        {
            bird.Fly();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var d = new Dragon();
            d.Crawl();
            d.Fly();

            Console.ReadLine();
        }
    }
}
