using System;
using System.Collections.Generic;

namespace AbstractFactory
{
    public interface IHotDrink
    {
        void Consume();
    }

    internal class Coffee : IHotDrink
    {
        public void Consume()
        {
            Console.WriteLine("Im drinking motherfucking coffee!");
        }
    }

    internal class Tea : IHotDrink
    {
        public void Consume()
        {
            Console.WriteLine("Im drinking motherfucking tea!");
        }
    }

    public interface IHotDrinkFactory
    {
        IHotDrink Prepare(int amount);
    }

    internal class TeaFactory : IHotDrinkFactory
    {

        public IHotDrink Prepare(int amount)
        {
            Console.WriteLine($"Im making {amount} ml of motherfucking tea!");
            return new Tea();
        }
    }

    internal class CoffeeFactory : IHotDrinkFactory
    {

        public IHotDrink Prepare(int amount)
        {
            Console.WriteLine($"Im making {amount} ml of motherfucking coffee!");
            return new Coffee();
        }
    }

    
    public class HotDrinkMachine
    {
        #region stari HotDrinkMachine
        //public enum AvailableDrink
        //{
        //    Coffee, Tea
        //}

        //public List<int> lista = new List<int>();

        //private Dictionary<AvailableDrink, IHotDrinkFactory> factories = new Dictionary<AvailableDrink, IHotDrinkFactory>();

        //public HotDrinkMachine()
        //{
        //    foreach (AvailableDrink drink in Enum.GetValues(typeof(AvailableDrink)))
        //    {

        //        //   on ovdje pravi jednu instancu factory koja moze biti ili CoffeeFactory ili TeaFactory a biće tipa IHotDrinkFactory
        //        //   jer je on univerzalan. pravimo univerzalnu preko Activator-a tako sto mu damo tip instance (fabrika caja ili kafe) 
        //        //   preko imena koje rucno pisemo u string dio po dio. 
        //        //   U ovom slucaju namespace. + drink + "Factory" jer znamo da smo ih tako pisali

        //        var factory = (IHotDrinkFactory)Activator.CreateInstance(Type.GetType("AbstractFactory." + drink + "Factory")); // Enum.GetName(typeof(AvailableDrink), drink) je stajalo umjesto drink zasto??? pojma nemam
        //        factories.Add(drink, factory);
        //    }
        //}

        //public IHotDrink MakeDrink(AvailableDrink drink, int amount)
        //{
        //    return factories[drink].Prepare(amount);
        //}
        #endregion

        private List<Tuple<string, IHotDrinkFactory>> factories = new List<Tuple<string, IHotDrinkFactory>>();

        //bas elegantan nacin

        public HotDrinkMachine()
        {
            foreach (var t in typeof(HotDrinkMachine).Assembly.GetTypes())
            {
                if (typeof(IHotDrinkFactory).IsAssignableFrom(t) && !t.IsInterface)
                {
                    factories.Add(Tuple.Create(
                        t.Name.Replace("Factory", String.Empty),
                        (IHotDrinkFactory)Activator.CreateInstance(t)
                        ));
                }
            }
        }

        public IHotDrink MakeDrink()
        {
            Console.WriteLine("List of available drinks:");
            for (int i = 1; i <= factories.Count; i++)
            {
                Console.WriteLine($"{i} : {factories[i-1].Item1}");
            }

            Console.WriteLine();
            Console.WriteLine("Pick one mofo");

            while (true)
            {
                string s;
                if ((s = Console.ReadLine()) != null && int.TryParse(s, out int i) && i > 0 && i <= factories.Count)
                {
                    Console.WriteLine();
                    Console.WriteLine("How much ml?");
                    s = Console.ReadLine();

                    if (s != null && int.TryParse(s, out int amount) && amount > 0)
                    {

                        return factories[i - 1].Item2.Prepare(amount);
                    }

                    Console.WriteLine("Idiot");

                }
                Console.WriteLine("Idiot");
            }

            
        }

    }




    class Program
    {
        static void Main(string[] args)
        {
            var hdm = new HotDrinkMachine();
            //var drink = hdm.MakeDrink(HotDrinkMachine.AvailableDrink.Coffee, 100);
            //drink.Consume();
            var drink = hdm.MakeDrink();
            drink.Consume();


            Console.Read();
        }
    }
}
