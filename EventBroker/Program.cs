using Autofac;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EventBroker
{
    public class Actor
    {
        protected EventBroker broker;

        public Actor(EventBroker broker)
        {
            this.broker = broker ?? throw new ArgumentNullException(nameof(broker));
        }
    }

    public class Player : Actor
    {
        public string Name { get; set; }
        public int GoalsScored { get; set; } = 0;

        public Player(EventBroker broker, string name) : base(broker)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(Name));

            broker.OfType<GoalScoredEvent>()
                .Subscribe(pe =>
                {
                    if (!pe.Name.Equals(name))
                    {
                        Console.WriteLine($"{name}: Well played {pe.Name}, it's your {pe.GoalsScored} goal.");
                    }
                });
            broker.OfType<RedCardEvent>()
                .Subscribe(pe =>
                {
                    if (!pe.Name.Equals(name))
                    {
                        Console.WriteLine($"{name}: Well played {pe.Name}, fuck that guy.");
                    }
                });
        }

        internal void Score()
        {
            GoalsScored++;
            broker.Publish(new GoalScoredEvent {Name = this.Name, GoalsScored = this.GoalsScored });
        }

        public void RedCard()
        {
            broker.Publish(new RedCardEvent { Name = this.Name, Violation = "insulting referee" });
        }
    }

    public class Coach : Actor
    {
        public Coach(EventBroker broker) : base(broker)
        {
            broker.OfType<GoalScoredEvent>()
                .Subscribe(pe =>
                {
                    if (pe.GoalsScored < 3)
                    {
                        Console.WriteLine($"coach: Well done {pe.Name}");
                    }
                }
                );

            broker.OfType<RedCardEvent>()
                .Subscribe(pe =>
                {
                    if (pe.Violation == "insulting referee")
                    {
                        Console.WriteLine($"coach: You are an idiot {pe.Name}");
                    }
                });
        }
    }

    public class PlayerEvent
    {
        public string Name { get; set; }
    }

    public class GoalScoredEvent : PlayerEvent
    {
        public int GoalsScored { get; set; }
    }

    public class RedCardEvent : PlayerEvent
    {
        public string Violation { get; set; }
    }

    public class EventBroker : IObservable<PlayerEvent>
    {
        Subject<PlayerEvent> subscriptions = new Subject<PlayerEvent>();


        public IDisposable Subscribe(IObserver<PlayerEvent> observer)
        {
            return subscriptions.Subscribe(observer);
        }

        public void Publish(PlayerEvent pe)
        {
            subscriptions.OnNext(pe);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var cb = new ContainerBuilder();
            cb.RegisterType<EventBroker>().SingleInstance();//singleton
            cb.RegisterType<Coach>();
            //posto player ima name u konstruktoru moramo mu eksplicitno reci kako da pravi igraca
            cb.Register<Player>((c, p) => 
            new Player(
                c.Resolve<EventBroker>(),
                p.Named<string>("name")
                )
            );

            using (var c = cb.Build())
            {
                var coach = c.Resolve<Coach>();
                var p1 = c.Resolve<Player>(new NamedParameter("name", "noob1"));
                var p2 = c.Resolve<Player>(new NamedParameter("name", "noob2"));

                p1.Score();
                p1.Score();
                p1.Score();
                p1.RedCard();
                p2.Score();
            }

            Console.ReadLine();
        }
    }
}
