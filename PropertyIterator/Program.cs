using System;
using System.Linq;

namespace PropertyIterator
{
    public class Creature
    {
        private int[] stats = new int[3];
        private int strength = 0;
        private int agility = 1;
        private int intelligence = 2;

        public int Strength { get => stats[strength]; set => stats[strength] = value; }
        public int Agility { get => stats[agility]; set => stats[agility] = value; }
        public int Intelligence { get => stats[intelligence]; set => stats[intelligence] = value; }

        public double AverageStat => stats.Average();


    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}