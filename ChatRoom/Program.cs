using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatRoom
{
    public class Person
    {
        public string Name;
        public Chatroom chatroom;
        private List<string> chatlog = new List<string>();

        public Person(string name)
        {
            Name = name;
        }

        public void Say(string message)
        {
            chatroom.Broadcast(Name, message);
        }

        public void PrivateMessage(string receiver, string message)
        {
            chatroom.Message(Name, receiver, message);
        }

        public void Receive(string sender, string message)
        {
            string s = $"{sender}: {message}";
            chatlog.Add(s);
            Console.WriteLine($"[{Name}'s chat session] {s}");
        }
    }

    public class Chatroom
    {
        private List<Person> people = new List<Person>();

        public void Join(Person p)
        {
            string joinMsg = $"{p.Name} joins the room.";
            Broadcast("room", joinMsg);
            p.chatroom = this;
            people.Add(p);
        }

        public void Broadcast(string source, string message)
        {
            foreach (var p in people)
            {
                if (p.Name != source)
                {
                    p.Receive(source, message);
                }
            }
        }

        public void Message(string sender, string receiver, string message)
        {
            people.FirstOrDefault(p => p.Name == receiver)?.Receive(sender, message);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var room = new Chatroom();

            var john = new Person("John");
            var jane = new Person("Jane");
            var simon = new Person("Simon");

            room.Join(john);
            room.Join(jane);

            john.Say("Hello, I'm gay.");
            jane.Say("Good for you.");

            room.Join(simon);
            simon.Say("Wazzup bitches?");
            jane.PrivateMessage(simon.Name, "Are u gay?");

            Console.ReadLine();
        }
    }
}
