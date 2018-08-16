using System;

namespace ObserverViaEvent
{
    public class MyArgs
    {
        public string adress;
        public string name;
    }

    public class Person
    {
        private string adress;
        private string name;

        public Person(string adress, string name)
        {
            this.adress = adress ?? throw new ArgumentNullException(nameof(adress));
            this.name = name ?? throw new ArgumentNullException(nameof(name));
        }


        //event koji slusamo
        public event EventHandler<MyArgs> FallsIll;
        //ovdje obavjestavamo sve koji slusaju da se event desio
        public void CatchCold()
        {
            FallsIll?.Invoke(this, new MyArgs {adress = adress, name = name }); //ako iko slusa zovemo invoke
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var p = new Person("Brooklin", "Jane");
            p.FallsIll += callDoctor; //sabskrajbujemo se za event, tj ovdje ga hvatamo
            //p.FallsIll -= callDoctor; //ako hocemo da se odjavimo
            p.FallsIll += giveMedicine;
            p.CatchCold();

            Console.ReadLine();
        }

        private static void giveMedicine(object sender, MyArgs e)
        {
            Console.WriteLine($"Medicine has been given to {e.name}");
        }

        private static void callDoctor(object sender, MyArgs e)
        {
            Console.WriteLine($"A doctor has been sent to {e.adress}");
        }
    }
}
