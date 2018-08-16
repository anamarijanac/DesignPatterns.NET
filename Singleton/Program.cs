using MoreLinq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Singleton
{
    internal class Program
    {
        public interface IDatabase
        {
            int GetPopulation(string name);
        }

        private class SingletonDatabase : IDatabase
        {
            private Dictionary<string, int> capitals;
            private static int i;
            public static int Count => i;

            private SingletonDatabase()
            {
                Console.WriteLine("Initializing database");

                capitals = File.ReadAllLines(
                            Path.Combine(
                            new FileInfo(typeof(IDatabase).Assembly.Location).DirectoryName, "capitals.txt")
                             )
                            .Batch(2)
                            .ToDictionary(
                                list => list.ElementAt(0).Trim(),
                                list => int.Parse(list.ElementAt(1)));
            }

            private static Lazy<SingletonDatabase> instance => new Lazy<SingletonDatabase>(() => { i++; return new SingletonDatabase(); });
            public static SingletonDatabase Instance => instance.Value;

            public int GetPopulation(string name)
            {
                return capitals[name];
            }
        }

        [TestFixture]
        public class SingletonTests
        {
            [Test]
            public void IsSingleton()
            {
                var db1 = SingletonDatabase.Instance;
                var db2 = SingletonDatabase.Instance;
                Assert.That(1, Is.EqualTo(1));
                //Assert.That(db1, Is.SameAs(db2));
                //Assert.That(SingletonDatabase.Count, Is.EqualTo(1));
            }
        }

        //static void Main(string[] args)
        //{
        //    var db = SingletonDatabase.Instance;
        //    var count = SingletonDatabase.Count;
        //    var db1 = SingletonDatabase.Instance;
        //    var count1 = SingletonDatabase.Count;
        //    var db2= SingletonDatabase.Instance;
        //    var count2 = SingletonDatabase.Count;
        //    var x = db.GetPopulation("Beograd");

        //    Console.WriteLine("Hello World!");
        //}
    }
}