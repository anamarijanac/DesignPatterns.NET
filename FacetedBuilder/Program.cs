using System;
using System.Collections.Generic;

namespace FacetedBuilder
{

    public class Person
    {
        //address
        public string Street, Postcode, City;

        //employment- zaposlenje
        public string CompanyName, Position;
        public int Income;

        public override string ToString()
        {
            return $"{Street}, {Postcode}, {City}. {CompanyName}, {Position}, {Income}$";
        }
    }
    //fasada, nije bilder nego 1.cuva referencu na osobu koja se pravi 2. daje nam pristup podbilderima
    public class PersonBuilder
    {
        //ovo je samo referenca
        protected Person person = new Person();
       
        //otkrivamo podbildere da bi ih vidjeli kad stavimo tačku na personjobbuilder
        public PersonJobBuilder Works => new PersonJobBuilder(person);
        public PersonAddressBuilder Lives => new PersonAddressBuilder(person);

        //dodajemo pravilo za kastovanje iz personBuilder u Person
        //implicit znaci implicitno kastovanje iliti =
        public static implicit operator Person(PersonBuilder pb) //trickery iliti zajebancija sistema 
        {
            List<int> a = new List<int>() { 2,1};
           int asd = a[0];
            asd = 23;
            var b = a[0];
            a[0] = 8;
            return pb.person;
        }
    }

    public class PersonJobBuilder : PersonBuilder
    {
        public PersonJobBuilder(Person person)
        {
            this.person = person; //stavljamo ga u polje koje smo nasledili

        }
        
        //sad pravimo fluent bildere

        public PersonJobBuilder At(string companyName)
        {
            person.CompanyName = companyName;
            return this;
        }

        public PersonJobBuilder AsA(string position)
        {
            person.Position = position;
            return this;
        }

        public PersonJobBuilder Earns(int income)
        {
            person.Income = income;
            return this;
        }
    }

    public class PersonAddressBuilder : PersonBuilder
    {
        public PersonAddressBuilder(Person person)
        {
            this.person = person;
        }

        public PersonAddressBuilder At(string street)
        {
            person.Street = street;
            return this;
        }

        public PersonAddressBuilder Postcode(string postcode)
        {
            person.Postcode = postcode;
            return this;
        }

        public PersonAddressBuilder In(string city)
        {
            person.City = city;
            return this;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var pb = new PersonBuilder();
            Person person = pb.
                Works.At("Zastava").AsA("Full-Stack Developer").Earns(960000).
                Lives.At("Cara Dusana").Postcode("11000").In("Beograd").
                Works.At("Uvijek zaboravim");
            Console.WriteLine(person.ToString());

            Console.ReadLine();
        }
    }
}
