using System;
using System.ComponentModel;

namespace ObservableProperties
{
    public class Market
    {
        public BindingList<float> prices = new BindingList<float>();

        public void Add(float price)
        {
            prices.Add(price);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var m = new Market();
            m.prices.ListChanged += (sender, EventArgs) =>
            {
                if (EventArgs.ListChangedType == ListChangedType.ItemAdded)
                {
                    float price = ((BindingList<float>)sender)[EventArgs.NewIndex];
                    Console.WriteLine($"the price is {price}");
                }
            };

            m.Add(57);
            Console.ReadLine();
        }
    }
}
