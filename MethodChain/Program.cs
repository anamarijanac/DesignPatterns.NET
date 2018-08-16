using System;
//ovo radi ali ne mozemo da undo modifiere koji smo dodali kreaturi
namespace MethodChain
{
    public class Creature
    {
        public string Name;
        public int Attack, Defense;

        public Creature(string name, int attack, int defense)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Attack = attack;
            Defense = defense;
        }

        public override string ToString()
        {
            return $"Name : {Name}, Attack : {Attack}, Defense : {Defense}";
        }
    }

    public class CreatureModifier
    {
        protected Creature creature;
        protected CreatureModifier next; //linked list

        public CreatureModifier(Creature creature)
        {
            this.creature = creature ?? throw new ArgumentNullException(nameof(creature));
        }

        public void Add(CreatureModifier cm)
        {
            if (next != null)  //ako sledeći postoji dodaj njemu cm kao sledeći
                next.Add(cm);
            else
                next = cm;
        }

        public virtual void handle() => next?.handle(); // ako postoji next uradi mu handle // tako aktiviramo sve cm
    }

    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Creature creature) : base(creature)
        {
        }

        public override void handle()
        {
            Console.WriteLine($"Doubling {creature.Name}'s attack.");
            creature.Attack *= 2;
            base.handle();
        }
    }

    public class IncreaseDefenseModifier : CreatureModifier
    {
        public IncreaseDefenseModifier(Creature creature) : base(creature)
        {
        }

        public override void handle()
        {
            Console.WriteLine($"Increasing {creature.Name}'s defense.");
            creature.Defense += 3;
            base.handle();
        }
    }



    class Program
    {
        static void Main(string[] args)
        {
            var goblin = new Creature("Goblin", 1, 2);
            Console.WriteLine(goblin);

            var root = new CreatureModifier(goblin);

            root.Add(new DoubleAttackModifier(goblin));

            //kako prekinuti lanac? 
            //dodajemo modifier koji zaustavlja handle() i posle njega se handle ne poziva() 
            root.Add(new NoBuffModiefier(goblin));
            root.Add(new IncreaseDefenseModifier(goblin)); //nece se odraditi

            root.handle(); //odradi handle svih dodatih modifiera
            Console.WriteLine(goblin);


            Console.ReadLine();  ;
        }
    }

    internal class NoBuffModiefier : CreatureModifier
    {
        public NoBuffModiefier(Creature creature) : base(creature)
        {
        }

        public override void handle()
        {
            //ne radimo nista pa se ovdje niz zaustavlja
        }
    }
}
