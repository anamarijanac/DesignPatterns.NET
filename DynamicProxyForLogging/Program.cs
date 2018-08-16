using ImpromptuInterface;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace DynamicProxyForLogging
{
    public interface IBankAccount
    {
        void Deposit(int amount);
        bool Withdraw(int amount);
        string ToString();
    }

    public class BankAccount : IBankAccount
    {
        private int balance;
        private int overdraftLimit = -200;

        public void Deposit(int amount)
        {
            balance += amount;
            Console.WriteLine($"Deposited {amount}. Balance is now {balance}.");
        }

        public bool Withdraw(int amount)
        {
            if (amount > balance - overdraftLimit)
            {
                Console.WriteLine("Not enough money.");
                return false;                
            }
            balance -= amount;
            Console.WriteLine($"Withdrew {amount}. Balance is now {balance}.");
            return true;
        }

        public override string ToString()
        {
            return $"Balance : {balance}.";
        }
    }

    public class Log<T> : DynamicObject where T : class, new()
    {
        private readonly T subject;
        private Dictionary<string, int> methodCallCount = new Dictionary<string, int>();

        public Log(T subject)
        {
            this.subject = subject ?? throw new ArgumentNullException(nameof(subject));
        }

        public static I As<I>() where I : class
        {
            if (!typeof(I).IsInterface)
                throw new ArgumentException("Must be interface.");
            return new Log<T>(new T()).ActLike<I>();
            //implementira interfejs kad napravimo instancu
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            try
            {
                Console.WriteLine($"Invoking {subject.GetType().Name}.{binder.Name} with arguments [{string.Join(',', args)}]");

                if (methodCallCount.ContainsKey(binder.Name))
                {
                    methodCallCount[binder.Name]++;
                }
                else
                {
                    methodCallCount.Add(binder.Name, 1);
                }

                result = subject.GetType().GetMethod(binder.Name).Invoke(subject, args);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public string Info
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var kv in methodCallCount)
                {
                    sb.AppendLine($"{kv.Key} has been called {kv.Value} times.");
                }
                return sb.ToString();
            }
            
        }

        public override string ToString()
        {
            return $"{Info} \n {subject}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var ba = new BankAccount();
            ba.Deposit(20);
            ba.Withdraw(230);
            ba.Withdraw(207);
            Console.WriteLine(ba);

            var ba1 = Log<BankAccount>.As<IBankAccount>();
            ba1.Deposit(20);
            ba1.Withdraw(230);
            ba1.Withdraw(207);
            Console.WriteLine(ba1);

            Console.ReadLine(); 
        }
    }
}
