using System;
using System.Collections;
using System.Collections.Generic;

namespace Iterator
{
    //cvor univerzalnog tipa T
    public class Node<T>
    {
        public T Value;
        public Node<T> Left, Right;
        public Node<T> Parent;

        public Node(T value)
        {
            Value = value;
        }

        public Node(T value, Node<T> left, Node<T> right) : this(value)
        {
            Left = left;
            Right = right;
            Left.Parent = Right.Parent = this;
        }
    }

    public class InOrderIterator<T> // mora i on biti univerzalan
    {
        private readonly Node<T> root;
        private bool yieldedstart;

        //referenca na sadasnji cvor
        public Node<T> Current { get; set; }

        public InOrderIterator(Node<T> root)
        {
            this.root = root;
            Current = root;
            //nadjemo najljevlji cvor
            while (Current.Left != null)
            {
                Current = Current.Left;
            }

        }

        public bool MoveNext()
        {
            //provjeravamo da li je element validan da mozemo da iteriramo preko njega
            if (!yieldedstart)
            {
                yieldedstart = true;
                return true;                
            }

            //trazimo da li ima desni nod od trenutnog
            if (Current.Right != null)
            {
                Current = Current.Right;
                //ako postoji nadjemo njegov najljevlji
                while (Current.Left != null)
                {
                    Current = Current.Left;
                }
                return true;
            }
            //ako nema desnog idemo na gore
            else
            {
                var p = Current.Parent;
                //dok je trenutni desno i postoji parrent idemo gore
                while (p != null && Current == p.Right)
                {
                    Current = p;
                    p = p.Parent;
                }
                Current = p;
                return Current != null;

            }

        }

        public void Reset()
        {

        }
    }

    //primjer metoda
    public class BinaryTree<T>
    {
        private Node<T> root;

        public BinaryTree(Node<T> root)
        {
            this.root = root;
        }

        public IEnumerable<Node<T>> InOrder
        {
            get
            {
                //ugnjezdeni metod
                IEnumerable<Node<T>> Traverse(Node<T> current)
                {
                    //ako postoji lijevi vrati ga, pa na to nalijepi parent, pa ako ima desni + rekurzivno to obavimo i za lijevi i desni
                    if (current.Left != null)
                    {
                        foreach (var left in Traverse(current.Left))
                        {
                            yield return left;
                        }
                    }
                    yield return current;
                    if (current.Right != null)
                    {
                        foreach (var right in Traverse(current.Right))
                        {
                            yield return right;
                        }
                    }
                }
                //ovdje koristimo metod - u geteru propertija
                foreach (var node in Traverse(root))
                {
                    yield return node;
                }
            }
        }

        public InOrderIterator<T> GetEnumerator()
        {
            return new InOrderIterator<T>(root);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            /*  pravimo drvo
             *      3
             *    /   \
             *   8     5
             */
             // hoćemo da ga iscitamo sa lijeva na desno 835

            var root = new Node<int>(3, new Node<int>(8), new Node<int>(5));

            //test za iterator
            var it = new InOrderIterator<int>(root);
            while (it.MoveNext())
            {
                Console.WriteLine(it.Current.Value);
            }
            //test za binarytree
            var bt = new BinaryTree<int>(root);
            foreach (var node in bt.InOrder)
            {
                Console.WriteLine(node.Value);
            }
            //ako nam treba samo jedan nacin za iteraciju mozemo ovako da pozovemo ako imamo:
            //klasu iterator koja u sebi ima property Current i bool metod MoveNext() 
            //i u klasi BinaryTree metod GetEnumerator, da bi BinaryTree bio enumerable
            foreach (var node in bt)
            {
                Console.WriteLine(node.Value);
            }
            Console.ReadLine();
        }
    }
}
