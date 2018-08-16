using Autofac;
using MoreLinq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Singleton2
{
    public interface IDatabase
    {
        int GetPopulation(string city);
    }

    public class SingletonDatabase : IDatabase
    {
        private Dictionary<string, int> capitals;

        private static int i;
        public static int Count => i;

        private SingletonDatabase()
        {
            i++;
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

        private static Lazy<SingletonDatabase> instance = new Lazy<SingletonDatabase>( () => new SingletonDatabase());//dajemo lambda izraz
        public static SingletonDatabase Instance => instance.Value; // ovdje se invokuje(poziva) lambda

        public int GetPopulation(string city)
        {
            return capitals[city];
        }
    } //rucno pravimo singleton

    public class OrdinaryDatabase : IDatabase
    {
        private Dictionary<string, int> capitals;


        public OrdinaryDatabase() 
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

        public int GetPopulation(string city)
        {
            return capitals[city];
        }
    } //obicna baza koju cemo pomocu delegata pretvoriti u singleton

    public class SingletonRecordFinder
    {
        public int GetTotatPopulation(IEnumerable<string> names)
        {
            int total = 0;
            foreach (var item in names)
            {
                total += SingletonDatabase.Instance.GetPopulation(item); //losa ideja za testiranje 
                //hardkodovanje instance
            }
            return total;
        }
    }

    //zato pravimo novu klasu u koju cemo moci ubaciti svakakve baze i dummy
    public class ConfigurablerecordFinder
    {
        IDatabase database;

        public ConfigurablerecordFinder(IDatabase database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public int GetTotalPopulation(IEnumerable<string> names)
        {
            int total = 0;
            foreach (var item in names)
            {
                total += database.GetPopulation(item); //univerzalna baza 
                
            }
            return total;
        }
    }

    //lazna baza da testiramo configurableRF
    public class DummyDatabase : IDatabase
    {
        public int GetPopulation(string city)
        {
            return new Dictionary<string, int>
            {
                ["alpha"] = 1,
                ["beta"] = 2,
                ["gamma"] = 3
            }[city];
        }
    }

    [TestFixture]
    public class SingletonTests
    {
        [Test]
        public void IsSingletonTest()
        {
            var db1 = SingletonDatabase.Instance;
            var db2 = SingletonDatabase.Instance;

            Assert.That(db1, Is.SameAs(db2));
            Assert.That(SingletonDatabase.Count, Is.EqualTo(1));
        }

        [Test] //hardcoded baza test
        public void SingletonTotalPopulationTest()
        {
            var srf = new SingletonRecordFinder();
            var names = new[] { "Kabul", "Bridgetown" };

            Assert.That(srf.GetTotatPopulation(names), Is.EqualTo(3289000 + 96578)); 
            //problem testiranje nije univerzalno, moramo da rucno pisemo rezultat
        }

        [Test] //dummy baza 
        public void ConfigurablePopulationTest()
        {
            var crf = new ConfigurablerecordFinder(new DummyDatabase());
            var names = new[] { "alpha", "gamma" };

            Assert.That(crf.GetTotalPopulation(names), Is.EqualTo(4));
        }

        [Test] //dependency injection singleton //  koristimo autofac iz nugeta da obavi DI
        public void DIPopulationTest()
        {
            var cb = new ContainerBuilder(); //kontejner

            //registrujemo tip OrdinaryDatabase kao singleton i jos kao idatabase jer nam to treba za crf dole
            //u prevodu rekli smo da kad god neko trazi ordinary database mi mu ga damo kao singleton
            cb.RegisterType<OrdinaryDatabase>().As<IDatabase>().SingleInstance();

            cb.RegisterType<ConfigurablerecordFinder>();
            using (var c = cb.Build())
            {
                var rf = c.Resolve<ConfigurablerecordFinder>();
                
            }

        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var db = SingletonDatabase.Instance;
            var x = db.GetPopulation("Kabul");
            var dbo = SingletonDatabase.Instance;

            Console.ReadLine();
        }
    }
}
