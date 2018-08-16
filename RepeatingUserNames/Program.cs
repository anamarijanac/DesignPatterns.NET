using JetBrains.dotMemoryUnit;
using JetBrains.dotMemoryUnit.Kernel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepeatingUserNames
{
    public class User
    {
        private string fullname;

        public User(string fullname)
        {
            this.fullname = fullname ?? throw new ArgumentNullException(nameof(fullname));
        }

    }

    [TestFixture]
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        [Test]
        public void TestUser()
        {
            //100 random imena i prezimena
            var firstNames = Enumerable.Range(0, 100).Select(_ => RandomString());//selektujemo 100 random stringova
            var lastNames = Enumerable.Range(0, 100).Select(_ => RandomString());

            //10 000 imena i prezimena
            var users = new List<User>();

            foreach (var fn in firstNames)
            {
                foreach (var ln in lastNames)
                {
                    users.Add(new User($"{fn} {ln}"));
                }
            }

            //garbage collector čisti iz memorije shit
            forceGC();

            //no money no fun!

            //if (dotMemoryApi.IsEnabled)
            //{
            //    dotMemory.Check(m => Console.WriteLine(m.SizeInBytes));
            //}
            

        }

        private void forceGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private object RandomString()
        {
            Random rand = new Random();
            return new string(Enumerable.Range(0, 10).
                Select(i => (char)('a' + rand.Next(26))).ToArray());//selektujemo 10 random karaktera i stavljamo ih u niz
        }
    }
}
