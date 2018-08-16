using System;

namespace InterfaceSegregationPrinciple
{
    public class Document
    {

    }

    public interface IMachine
    {
        void Print(Document d);
        void Scan(Document d);
        void Fax(Document d);
    }
    //radi dobro
    public class MultFunctionPrinter : IMachine
    {
        public void Fax(Document d)
        {
            //
        }

        public void Print(Document d)
        {
            //
        }

        public void Scan(Document d)
        {
            //
        }
    }

    //problem : moramo da implementiramo sve metode a hoćemo samo da štampamo
    public class OldFashionedPrinter : IMachine
    {
        public void Fax(Document d)
        {
            throw new NotImplementedException();
        }

        public void Print(Document d)
        {
            //
        }

        public void Scan(Document d)
        {
            throw new NotImplementedException();
        }
    }

    //rijesenje : razbijemo IMachine u vise malih interfejsa
    public interface IPrinter
    {
        void Print(Document d);
    }

    public interface IScanner
    {
        void Scan(Document d);
    }

    public interface IFaxMachine
    {
        void Fax(Document d);
    }

    public class Printer : IPrinter
    {
        public void Print(Document d)
        {
            //
        }
    }

    public class Fotocopier : IPrinter, IScanner
    {
        public void Print(Document d)
        {
            //
        }

        public void Scan(Document d)
        {
            //
        }
    }

    //možemo opet spojiti ta 3 manja ako nam trebaju za nesto zajedno
    public interface IThreeInOne : IScanner, IPrinter, IFaxMachine
    {

    }

    public class ThreeInOne : IThreeInOne
    {
        public void Fax(Document d)
        {
            //
        }

        public void Print(Document d)
        {
            //
        }

        public void Scan(Document d)
        {
            //
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
