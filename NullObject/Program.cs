using Autofac;
using ImpromptuInterface;
using System;
using System.Dynamic;
//zelimo da mozemo da ne zadajemo parametar u konstruktor, ili da on moze bit null
namespace NullObject
{
    public interface ILog
    {
        void info(string msg);
        void warn(string msg);
    }

    public class ConsoleLog : ILog
    {
        public void info(string msg)
        {
            Console.WriteLine(msg);
        }

        public void warn(string msg)
        {
            Console.WriteLine($"warning: {msg}");
        }
    }
    //nullobject
    public class NullLog : ILog
    {
        public void info(string msg)
        {
            
        }

        public void warn(string msg)
        {
            
        }
    }

    public class BankAccount
    {
        private ILog log;
        private int balance;

        public BankAccount(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public void Deposit(int amount)
        {
            balance += amount;
            string msg = $"deposited {amount}. balance is now {balance}";
        }
    }

    #region DinamickiNullObject
    public class Null<interfaceT> : DynamicObject where interfaceT : class //pravi null instancu bilo kog interfejsa
    {
        public static interfaceT Instance
        {
            get
            {
                return new Null<interfaceT>().ActLike<interfaceT>(); //impromptuinterface iz nuget, null od tog interfejsa se folira da je taj interfejs
            }
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = Activator.CreateInstance(binder.ReturnType);
            return true; //posto moramo vratiti bool
        }
        //da se pobrinemo da kad napravimo null instancu ona nece nista raditi


    }

    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            var log = new ConsoleLog();
            var ba = new BankAccount(log);

            //sta ako ne zelimo log?
            var noLog = new NullLog();
            var ba2 = new BankAccount(noLog);


            //vjezbam dependency injection //nuget autofac

            var cb = new ContainerBuilder();
            cb.RegisterType<BankAccount>();
            cb.RegisterType<NullLog>().As<ILog>();

            using (var c = cb.Build())
            {
                var ba3 = c.Resolve<BankAccount>();
                ba3.Deposit(50);
            }            

            //test za dinamicki null objekat
            var nula = Null<ILog>.Instance;
            var ba4 = new BankAccount(nula);
            ba4.Deposit(75);

            Console.ReadLine();
        }
    }
}
