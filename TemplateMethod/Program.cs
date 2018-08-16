using System;

namespace TemplateMethod
{
    //sve sto je zajednicko za igre je u klasi game, u njenoj djeci se kastimizira
    public abstract class Game
    {
        //template method
        public void Run()
        {
            Start();
            while (!HaveWinner)
            {
                TakeTurn();
            }
            Console.WriteLine($"player {WinningPlayer} wins");
        }

        protected readonly int numberOfPlayers;
        protected int currentPlayer;

        protected Game(int numberOfPlayers)
        {
            this.numberOfPlayers = numberOfPlayers;
        }

        protected abstract void Start();
        protected abstract void TakeTurn();

        protected abstract bool HaveWinner { get; }
        protected abstract int WinningPlayer { get; }

    }

    public class Chess : Game
    {
        public Chess() : base(2)
        {
        }

        private int turn = 1;
        private int maxTurns = 20;

        protected override bool HaveWinner => turn == maxTurns;

        protected override int WinningPlayer => currentPlayer;

        protected override void Start()
        {
            Console.WriteLine($"startingf a game of chess with {numberOfPlayers} players");
        }

        protected override void TakeTurn()
        {
            Console.WriteLine($"{turn}. turn played by player {currentPlayer}");
            turn++;
            currentPlayer = (currentPlayer + 1) % numberOfPlayers;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var chess = new Chess();
            chess.Run();

            Console.ReadLine();
        }
    }
}
