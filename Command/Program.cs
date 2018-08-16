using System;
using System.Collections.Generic;
using System.Linq;

namespace Command
{
    public class BankAccount
    {
        private int balance;
        private int overdraftLimit = -500;

        internal void Deposit(int amount)
        {
            balance += amount;
            Console.WriteLine($"Deposited {amount}. Balance is now {balance}.");
        }

        internal bool Withdraw(int amount)
        {
            if (amount > balance - overdraftLimit)
            {
                Console.WriteLine("Not enough funds.");
                return false;
            }
            else
            {
                balance -= amount;
                Console.WriteLine($"Withdrew {amount}. Balance is now {balance}.");
                return true;
            }
        }
    }

    public interface ICommand
    {
        void call();
        void undo();
    }

    public class BankAccountCommand : ICommand
    {
        private bool suceeded;

        private BankAccount account;

        public enum Action
        {
            Deposit, Withdraw
        }

        private Action action;
        private int amount;

        public BankAccountCommand(BankAccount account, Action action, int amount) // sve sto nam treba: kome? BankAccount, sta? Action, koliko? amount
        {
            this.account = account ?? throw new ArgumentNullException(nameof(account));
            this.action = action;
            this.amount = amount;
        }

        public void call()
        {
            switch (action)
            {
                case Action.Deposit:
                    account.Deposit(amount);
                    suceeded = true;
                    break;
                case Action.Withdraw:
                    suceeded = account.Withdraw(amount);
                    break;
                default:
                    break;
            }
        }

        public void undo()
        {
            //deposit i withdraw rade suprotne stvari
            if (!suceeded) return;
            switch (action)
            {
                case Action.Deposit:
                    account.Withdraw(amount);
                    break;
                case Action.Withdraw:
                    account.Deposit(amount);
                    break;
                default:
                    break;
            }

        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var ba = new BankAccount();
            var bac1 = new BankAccountCommand(ba, BankAccountCommand.Action.Deposit, 300);
            var bac2 = new BankAccountCommand(ba, BankAccountCommand.Action.Withdraw, 900);

            var commands = new List<BankAccountCommand>();
            commands.Add(bac1);
            commands.Add(bac2);

            foreach (var c in commands)
            {
                c.call();
            }

            foreach (var c in Enumerable.Reverse(commands))
            {
                c.undo();
            }

            Console.ReadLine();
        }
    }
}
