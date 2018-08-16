using System;
using System.Collections.Generic;

namespace OpenClosePrinciple
{
    public enum Color
    {
        Red, Blue, Green
    }

    public enum Size
    {
        Smal, Medium, Large
    }

    public class Product
    {
        public String Name;
        public Size Size { get; set; }
        public Color Color { get; set; }

        public Product(string name, Size size, Color color)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Size = size;
            Color = color;
        }


    }
    #region Stari Nacin
    public class ProductFilter
    {
        public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
        {
            foreach (var p in products)
            {
                if (p.Size == size)
                    yield return p;
            }
        }

        public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
        {
            foreach (var p in products)
            {
                if (p.Color == color)
                    yield return p;
            }
        }
    }
    #endregion

    #region Novi Nacin (koristi specification pattern)
    public interface ISpecification<T>
    {
        bool IsSatisfied(T t); //postoji metod isSatisfied i posto smo stavili T možemo kad predefinisemo staviti bilo koji tip
    }

    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);//postoji Filter koji uzima ienum bilo kog tipa
    }

    public class ColorSpecification : ISpecification<Product>
    {
        private Color color;

        public ColorSpecification(Color color)
        {
            this.color = color;
        }

        public bool  IsSatisfied(Product t)
        {
            return t.Color == color;
        }
    }

    public class SizeSpecification : ISpecification<Product>
    {
        private Size Size;

        public SizeSpecification(Size size)
        {
            Size = size;
        }

        public bool IsSatisfied(Product t)
        {
            return t.Size == Size;
        }
    }

    public class AndSpecification<T> : ISpecification<T>
    {
        ISpecification<T> first, second;

        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            this.first = first ?? throw new ArgumentNullException(nameof(first));
            this.second = second ?? throw new ArgumentNullException(nameof(second));
        }

        public bool IsSatisfied(T t)
        {
            return first.IsSatisfied(t) && second.IsSatisfied(t);
        }
    }

    public class BetterFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
        {
            foreach (var item in items)
            {
                if (spec.IsSatisfied(item))
                {
                    yield return item;
                }
            }
        }
    }

    #endregion

    public class Program
    {
        static void Main(string[] args)
        {
            var apple = new Product("Apple", Size.Smal, Color.Green);
            var tree = new Product("Tree", Size.Medium, Color.Green);
            var house = new Product("House", Size.Large, Color.Blue);

            Product[] products = { apple, tree, house };
            ProductFilter pf = new ProductFilter();
            var bf = new BetterFilter();

            Console.WriteLine("Green products (old): ");            
            foreach (var p in pf.FilterByColor(products, Color.Green))
                Console.WriteLine(p.Name);

            Console.WriteLine("Green products (new): ");
            foreach (var item in bf.Filter(products, new ColorSpecification(Color.Green)))
                Console.WriteLine(item.Name);

            Console.WriteLine("What's big and blue?");
            foreach (var item in bf.Filter(products, new AndSpecification<Product>(new ColorSpecification(Color.Blue), new SizeSpecification(Size.Large))))
            {
                Console.WriteLine(item.Name);
            }

            Console.ReadLine();
        }
    }
}
