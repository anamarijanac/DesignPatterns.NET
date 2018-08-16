using System;
using System.Collections.Generic;

namespace PropertyProxy
{
    /*prave klasu Property<T> univerzalnog tipa T, 
     * da ne bi storovali duplikate bilo kog objekta koji prave 
     * oni ga ubace u property koji provjeri da taj property vec postoji takav
     */
    public class Property<T> : IEquatable<Property<T>> where T: new()
    {
        private T value;
        public T Value {
            get => value;
            set {
                if (Equals(this.value, value)) return;
                Console.WriteLine("Assigning value.");
                this.value = value;
            }
        }

        public Property() : this(Activator.CreateInstance<T>())
        {

        }

        public Property(T value)
        {
            this.value = value;

        }

        //implicitna konverzija
        public static implicit operator T(Property<T> property)
        {
            return property.value; //int n = new Property<int>(2);
        }

        public static implicit operator Property<T>(T value)
        {
            return new Property<T>(value); //Property<int> p = 123;
        }
        //autogenerisan == , != i Equals()
        public static bool operator ==(Property<T> property1, Property<T> property2)
        {
            return EqualityComparer<Property<T>>.Default.Equals(property1, property2);
        }


        public static bool operator !=(Property<T> property1, Property<T> property2)
        {
            return !(property1 == property2);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Property<T>);
        }

        public bool Equals(Property<T> other)
        {
            return other != null &&
                   EqualityComparer<T>.Default.Equals(value, other.value);
        }

        public override int GetHashCode()
        {
            return -1584136870 + EqualityComparer<T>.Default.GetHashCode(value);
        }
    }

    public class Creature 
    {
        private Property<int> agility = new Property<int>();
        public int Agility {
            get => agility.Value;
            set => agility.Value = value;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int n = new Property<int>(2);
            Property<int> p = 123;

            var c = new Creature();
            c.Agility = 10;
            c.Agility = 10;

            Console.ReadLine();
        }
    }
}
