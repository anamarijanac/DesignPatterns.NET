using System;
using System.Collections.Generic;

/*emuliramo telefon
 * moze imati 4 stanja: slobodna veza, biranje broja, u pozivu, hold
 * i ima dogadjaje koji okidaju prelazak u drugo stanje
 */

namespace HandmadeStateMachine
{
    public enum State
    {
        Offhook,
        Connecting,
        Connected,
        OnHold
    }

    public enum Trigger
    {
        CallDialed,
        Hungup,
        CallConnected,
        PlacedOnHold,
        TakenOffHold
    }

    class Program
    {
        //ovo je state machine
        private static Dictionary<State, List<(Trigger, State)>> rules = new Dictionary<State, List<(Trigger, State)>>
        {
            [State.Offhook] = new List<(Trigger, State)>
            {
                (Trigger.CallDialed, State.Connecting) // iz ofhook stanja(slobodna veza) prelazimo u konektovanje okidacem callDialed
            },
            [State.Connecting] = new List<(Trigger, State)>
            {
                (Trigger.CallConnected, State.Connected),
                (Trigger.PlacedOnHold, State.OnHold)
            },
            [State.Connected] = new List<(Trigger, State)>
            {
                (Trigger.Hungup, State.Offhook)
            },
            [State.OnHold] = new List<(Trigger, State)>
            {
                (Trigger.TakenOffHold, State.Connecting)
            }
        };


        static void Main(string[] args)
        {
            var state = State.Offhook;

            while (true)
            {
                Console.WriteLine($"the phone is currently {state}.");
                Console.WriteLine("select a trigger:");

                for (int i = 0; i < rules[state].Count; i++)
                {
                    var (t, _) = rules[state][i]; //_ znaci da ignorisemo taj argument
                    Console.WriteLine($"{i} {t}");
                }
                int input = int.Parse(Console.ReadLine());

                //jos jasnije: uzimamo tuple koji se nalazi u itom clanu liste koju smo dobili iz dictionary za keyword state
                // i ignorisemo t, samo nas s zanima
                var (_, s) = rules[state][input];
                state = s;
            }

            Console.ReadLine();
        }
    }
}
