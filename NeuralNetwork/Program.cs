using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

//upotreba composite design pattern a
//simulacija kako neuroni kod covjeka uče, najbolji nacin za učenje kompjutera

namespace NeuralNetwork
{
    public static class ExtensionMethods
    {
        public static void ConnectTo(this IEnumerable<Neuron> self, IEnumerable<Neuron> other)
        {
            if (ReferenceEquals(self, other))
                return;
            foreach (var from in self)
            {
                foreach (var to in other)
                {
                    from.Out.Add(to);
                    to.In.Add(from);
                }
            }
        }
    }
    //da bi neuron mogao koristiti zajednicke metode pretvaramo ga u IEnumerable. napomena- Collection<> je isto IEnumerable
    public class Neuron : IEnumerable<Neuron>
    {
        public float Value;
        //public List<Neuron> In = new List<Neuron>(), Out = new List<Neuron>();

        private Lazy<List<Neuron>> incoming = new Lazy<List<Neuron>>();
        public List<Neuron> In => incoming.Value;

        private Lazy<List<Neuron>> napolje = new Lazy<List<Neuron>>();
        public List<Neuron> Out => napolje.Value;

        public IEnumerator<Neuron> GetEnumerator()
        {
            yield return this; //ovako ce da vrati IEnumerable sa samo jednim članom : this
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();//po defaultu se ovako radi
        }
    }

    public class NeuronLayer : Collection<Neuron>
    {

    }

    class Program
    {
        static void Main(string[] args)
        {
            var neuron1 = new Neuron();
            var neuron2 = new Neuron();

            neuron1.ConnectTo(neuron2);

            var neuronLayer1 = new NeuronLayer();
            var neuronLayer2 = new NeuronLayer();

            neuron1.ConnectTo(neuronLayer1);
            neuronLayer1.ConnectTo(neuronLayer2);
            neuronLayer2.ConnectTo(neuron2);


            Console.ReadLine();
        }
    }
}
