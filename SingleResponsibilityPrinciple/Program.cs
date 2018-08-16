using System;
using System.Collections.Generic;
using System.IO;

namespace SingleResponsibilityPrinciple
{
    public class Journal
    {
        private readonly List<string> entries = new List<string>();
        private static int count = 0;

        public int addEntry(string text)
        {
            entries.Add($"{++count} : {text}");
            return count;
        }

        public void removeEntry(int index)
        {
            entries.RemoveAt(index);
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, entries);
        }

        //public void save(string filename)
        //{
        //    file.writealltext(filename, tostring());
        //}

        //public void load(string filename) { }

        //jedna klasa jedna odgovornost, 
        //pa sve sto ima veze sa cuvanjem fajlova(persistance) prebacujemo u novu klasu

    }

    public class Persistance
    {
        public void saveToFile(Journal j, string filename, bool overwrite = false)
        {
            if (overwrite || !File.Exists(filename))
            {
                File.WriteAllText(filename, j.ToString());
            }
            
        }

        public void load(string filename) { }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Journal j = new Journal();
            j.addEntry("I cried today.");
            j.addEntry("I ate a bug.");

            Persistance p = new Persistance();
            var filename = @"c:\temp\journal.txt";
            p.saveToFile(j, filename, true);
            Console.WriteLine(j);
            Console.ReadLine();
        }
    }
}
