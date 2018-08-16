using System;
using System.Collections.Generic;
using System.Linq;

namespace DependencyInversionPrinciple
{
    public enum Relationship
    {
        Parent, Child, Sibling
    }

    public class Person
    {
        public string Name;
    }

    // low-lvl klasa
    public class Relationships : IRelationshipBrowser
    {
        private List<(Person, Relationship, Person)> relations
            = new List<(Person, Relationship, Person)>();

        public List<(Person, Relationship, Person)> Relations => relations;

        public void AddParentChild(Person parent, Person child)
        {
            relations.Add((parent, Relationship.Parent, child));
            relations.Add((child, Relationship.Child, parent));
        }

        public void AddSiblings(Person sibling1, Person sibling2)
        {
            relations.Add((sibling1, Relationship.Sibling, sibling2));
            relations.Add((sibling2, Relationship.Sibling, sibling1));
        }

        //implementacija interfejsa
        public IEnumerable<Person> FindAllChildrenOf(Person person)
        {
            foreach (var r in relations.Where(x => x.Item1.Name == person.Name && x.Item2 == Relationship.Parent))
            {
                yield return r.Item3;
            }
        }

        public IEnumerable<Person> FindAllSiblingsOf(Person person)
        {
            foreach (var r in relations.Where(x => x.Item1.Name == person.Name && x.Item2 == Relationship.Sibling))
            {
                yield return r.Item3;
            }
        }
    }

    //bolji nacin
    public interface IRelationshipBrowser
    {
        IEnumerable<Person> FindAllChildrenOf(Person person);
        IEnumerable<Person> FindAllSiblingsOf(Person person);

    }

    class Program
    {
        //stari
        //public Program(Relationships relationships)
        //{
        //    //ovo radi sada ali ako izmijenimo Relationships klasu prestaće da radi 
        //    var relations = relationships.Relations;

        //    foreach (var r in relations.Where(x => x.Item1.Name == "Dick" && x.Item2 == Relationship.Parent))
        //    {
        //        Console.WriteLine("Dick has a child called " + r.Item3.Name);
        //    }

        //    foreach (var r in relations.Where(x => x.Item1.Name == "Dick" && x.Item2 == Relationship.Sibling))
        //    {
        //        Console.WriteLine("Dick has a sibling called " + r.Item3.Name);
        //    }

        //}
        //novi
        public Program(IRelationshipBrowser relationships, List<Person> people)
        {
            foreach (var person in people)
            {
                foreach (var ch in relationships.FindAllChildrenOf(person))
                {
                    Console.WriteLine(person.Name + " has a child called " + ch.Name);
                }

                foreach (var ch in relationships.FindAllSiblingsOf(person))
                {
                    Console.WriteLine(person.Name + " has a sibling called " + ch.Name);
                }
            }

        }

        static void Main(string[] args)
        {
            var parent = new Person { Name = "Dick" };
            var child = new Person { Name = "Tom" };
            var sibling1 = new Person { Name = "Harry" };
            var sibling2 = new Person { Name = "Sally" };

            var persons = new List<Person>();
            persons.Add(parent);
            persons.Add(child);
            persons.Add(sibling1);
            persons.Add(sibling2);

            var relationships = new Relationships();

            relationships.AddParentChild(parent, child);
            relationships.AddSiblings(parent, sibling1);
            relationships.AddSiblings(parent, sibling2);
            relationships.AddSiblings(sibling1, sibling2);

            
            new Program(relationships, persons);  //horrible :D

            Console.ReadLine();
        }
    }
}
