using System;

namespace ProtectionProxy
{
    public interface ICar
    {
        void Drive();
    }

    public class Car : ICar
    {
        public void Drive()
        {
            Console.WriteLine("car is being driven");
        }
    }

    //treba nam neka zastita, da ne bi premladi vozaci vozili
    //radije nego da mijenjamo auto, dodajemo klasu auto proxy
    public class CarProxy : ICar   
    {
        private Driver driver;
        private Car car = new Car();

        public CarProxy(Driver driver)
        {
            this.driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }

        public void Drive()
        {
            if (driver.Age < 16)
            {
                Console.WriteLine("Too young to drive.");
            }
            else
            {
                car.Drive();
            }
        }
    }

    public class Driver
    {
        public int Age;

        public Driver(int age)
        {
            Age = age;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var cp = new CarProxy(new Driver(16));
            cp.Drive();

            Console.ReadLine();
        }
    }
}
