using System;
using System.Collections.Generic;

namespace Memento
{
    public class BankAccount
    {
        private int balance { get; set; }
        private List<Mem3nto> changes = new List<Mem3nto>();
        private int current;

        public BankAccount(int balance)
        {
            this.balance = balance;
            var m = new Mem3nto(balance);
            changes.Add(m);
        }

        public Mem3nto Deposit(int amount)
        {
            balance += amount;
            var m = new Mem3nto(balance);
            changes.Add(m);
            ++current;
            return m;
        }

        public Mem3nto Restore(Mem3nto m)
        {
            if (m != null)
            {
                balance = m.Balance;
                changes.Add(m);
                return m;
            }
            else return null;
        }

        public Mem3nto Undo()
        {
            if (current > 0)
            {
                var m = changes[--current];
                balance = m.Balance;
                return m;
            }
            else return null;
        }

        public Mem3nto Redo()
        {
            if (current + 1 < changes.Count)
            {
                var m = changes[++current];
                balance = m.Balance;
                return m;
            }
            else return null;
        }

        public override string ToString()
        {
            return $"balance: {balance}";
        }
    }

    public class Mem3nto
    {
        public int Balance { get; private set; }

        public Mem3nto(int balance)
        {
            Balance = balance;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var ba = new BankAccount(100);
            var m1 = ba.Deposit(50);
            var m2 = ba.Deposit(25);
            Console.WriteLine(ba);

            ba.Restore(m1);
            Console.WriteLine(ba);

            ba.Restore(m2);
            Console.WriteLine(ba);

            ba.Undo();
            Console.WriteLine(ba);

            ba.Undo();
            Console.WriteLine(ba);

            ba.Redo();
            Console.WriteLine(ba);


            Console.ReadLine();
        }
    }
}
