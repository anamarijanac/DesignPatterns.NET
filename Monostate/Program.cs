using System;

namespace Monostate
{
    //ideja je da damo da se napravi koliko hoces istanci ali sve imaju iste staticke clanove
    //zajebali smo sistem, kome treba singleton 
    public class CEO
    {
        //staticki privatni propertiji
        private static string name;
        private static int age;


        public string Name
        {
            get => name;
            set => name = value;
        }

        public int Age
        {
            get => age;
            set=> age = value;
        }

        public override string ToString()
        {
            return $"{name} : {age}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var ljubo = new CEO {Name = "Ljubo", Age = 32 };
            Console.WriteLine(ljubo);
            var ana = new CEO { Name = "Ana" };

            Console.WriteLine(ljubo);
            Console.WriteLine(ana);

            Console.ReadLine();
        }
    }
}
