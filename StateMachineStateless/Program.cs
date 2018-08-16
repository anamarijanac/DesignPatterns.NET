using Stateless;
using System;

namespace StateMachineStateless
{
    public enum SexualHealth
    {
        NonReproductive,
        Reproductive, 
        Pregnant
    }

    public enum Triger
    {
        ReachPuberty,
        GiveBirth,
        HaveAbortion,
        HaveUnprotectedSex,
        Historectomy
    }

    class Program
    {
        static void Main(string[] args)
        {
            var machine = new StateMachine<SexualHealth, Triger>(SexualHealth.NonReproductive);

            machine.Configure(SexualHealth.NonReproductive).
                Permit(Triger.ReachPuberty, SexualHealth.Reproductive);

            machine.Configure(SexualHealth.Reproductive)
                .Permit(Triger.HaveUnprotectedSex, SexualHealth.Pregnant)
                .Permit(Triger.Historectomy, SexualHealth.NonReproductive);

            machine.Configure(SexualHealth.Pregnant)
                .Permit(Triger.HaveAbortion, SexualHealth.Reproductive)
                .Permit(Triger.GiveBirth, SexualHealth.Reproductive);
            //moze da se doda uslov za tranziciju .Permitif

            Console.ReadLine();
        }
    }
}
