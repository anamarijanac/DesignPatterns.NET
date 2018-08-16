using System;
//bolji nacin 
namespace BrokerChain
{
    public class Game //posrednik - mediator pattern
    {
        //upiti
        public event EventHandler<Query> Queries;

        //api koji opaljuje upite
        public void PerformQuery(object sender, Query q)
        {
            Queries?.Invoke(sender, q);
        }
    }

    public class Query
    {
        //od koga zelim
        public string CreatureName;
        
        public enum Argument //skup stvari na koje mozemo stavljat modifiere
        {
            Attack, Defense
        }
        //sta zelim
        public Argument WhatToQuery;

        //rezultat upita
        public int Value;

        public Query(string creatureName, Argument whatToQuery, int value)
        {
            CreatureName = creatureName ?? throw new ArgumentNullException(nameof(creatureName));
            WhatToQuery = whatToQuery;
            Value = value;
        }
    }

    public class Creature
    {
        private Game game;
        public string Name;
        private int attack, defense;

        public Creature(Game game, string name, int attack, int defense)
        {
            this.game = game ?? throw new ArgumentNullException(nameof(game));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            this.attack = attack;
            this.defense = defense;
        }

        public int Attack {
            get
            {
                var q = new Query(Name, Query.Argument.Attack, attack);
                game.PerformQuery(this, q);
                return q.Value;
            }
        }

        public int Defense
        {
            get
            {
                var q = new Query(Name, Query.Argument.Defense, defense);
                game.PerformQuery(this, q);
                return q.Value;
            }
        }

        public override string ToString()
        {
            return $"name : {Name}, attack : {Attack}, defense : {Defense}";
        }
    }

    public abstract class CreatureModifier : IDisposable //da bi mogli ono using
    {
        protected Game game;
        protected Creature creature;

        protected CreatureModifier(Game game, Creature creature)
        {
            this.game = game ?? throw new ArgumentNullException(nameof(game));
            this.creature = creature ?? throw new ArgumentNullException(nameof(creature));
            game.Queries += Handle;
        }

        public abstract void Handle(object sender, Query q);

        public void Dispose()
        {
            game.Queries -= Handle;
        }
    }

    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Game game, Creature creature) : base(game, creature)
        {
        }

        public override void Handle(object sender, Query q)
        {
            if (q.CreatureName == creature.Name && q.WhatToQuery == Query.Argument.Attack)
            {
                q.Value *= 2;
            }
        }
    }

    public class IncreaseDefenseModifier : CreatureModifier
    {
        public IncreaseDefenseModifier(Game game, Creature creature) : base(game, creature)
        {
        }

        public override void Handle(object sender, Query q)
        {
            if (q.CreatureName == creature.Name && q.WhatToQuery == Query.Argument.Defense)
            {
                q.Value += 3;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();
            var fairy = new Creature(game, "Fairy", 5, 1);

            Console.WriteLine(fairy);

            using (var dam = new DoubleAttackModifier(game, fairy))
            {
                Console.WriteLine(fairy);
            }
            Console.WriteLine(fairy);

            using (var idm = new IncreaseDefenseModifier(game, fairy))
            {
                Console.WriteLine(fairy);
            }
            Console.WriteLine(fairy);

            Console.ReadLine();
        }
    }
}
