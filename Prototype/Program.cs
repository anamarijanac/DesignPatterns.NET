using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Prototype
{
    //hocemo sve da automatizujemo pa ovo nije dovoljno dobro
    //public interface IPrototype<T>
    //{
    //    T DeepCopy();
    //}

    public static class ExtensionMethods
    {
        public static T DeepCopy<T>(this T self)
        {
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();

            formatter.Serialize(stream, self); //pomocu formatera upisujemo objekat self u stream
            stream.Seek(0, SeekOrigin.Begin); // premotamo strim na pocetak da ga mozemo kopirat
            object copy = formatter.Deserialize(stream);
            stream.Close();

            return (T)copy;

        }

        //ako koristimo binary formatter moramo da stavimo [Serializable] ako koristimo xml jok

        public static T DeepXmlCopy<T>(this T self)
        {
            using (var ms = new MemoryStream())
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(ms, self);
                ms.Position = 0;
                object copy = serializer.Deserialize(ms);
                return (T)copy;
            }
            
        }
    }
    //[Serializable]
    public class Person //IPrototype<Person>
    {
        public string[] Names = new string[3];
        public Address Address;

        public Person(string[] names, Address address)
        {
            Names = names ?? throw new ArgumentNullException(nameof(names));
            this.Address = address ?? throw new ArgumentNullException(nameof(address));
        }

        public Person()
        {
                    
        }

        //ovo je kopikonstruktor
        private Person(Person drugi)
        {
            for (int i = 0; i < drugi.Names.Length; i++)
            {
                Names[i] = drugi.Names[i];
            }
            this.Address = drugi.Address.DeepCopy();
        }

        public override string ToString()
        {
            return $"{String.Join(" ", Names)}, {Address}";
        }

        //public Person DeepCopy()
        //{
        //    return new Person(this);
        //}
    }

    //[Serializable]
    public class Address //IPrototype<Address>
    {
        public string Street;
        public string City;

        public Address()
        {

        }

        private Address(Address address)
        {
            this.Street = address.Street;
            this.City = address.City;
        }

        public Address(string street, string city)
        {
            Street = street ?? throw new ArgumentNullException(nameof(street));
            City = city ?? throw new ArgumentNullException(nameof(city));
        }


        public override string ToString()
        {
            return $"{Street}, {City}";
        }

        //public Address DeepCopy()
        //{
        //    return new Address(this);
        //}
    }

    class Program
    {
        static void Main(string[] args)
        {
            Person p1 = new Person(new[] { "Ana", "Marijanac", "Vujicic" }, new Address("Cara Dusana", "Beograd"));

            //ne mere ovo zato pravimo kopikonstruktore

            //Person p2 = p1;
            //Person p2 = new Person(p1);

            Person p2 = p1.DeepXmlCopy();

            p2.Names[0] = "Ljubo";

            Console.WriteLine(p1);
            Console.WriteLine(p2);

            Console.ReadLine();
        }
    }
}
