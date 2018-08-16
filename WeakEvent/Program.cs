using System;
/*
 * problem: kada zelimo da unistimo objekat koji se subskrajbovao, nece se unistiti dok je sabskrajbovan
 */

public class Button
{
    public event EventHandler Clicked; //deklarisem event

    public void Fire()
    {
        Clicked?.Invoke(this, EventArgs.Empty); //kazem kad se opaljuje
    }
}

public class Window 
{
    public Window(Button button)
    {
        button.Clicked += OnClick;

    }

    public void OnClick(object sender, EventArgs e)
    {
        Console.WriteLine("Button clicked");
    }

    ~Window() //destruktor
    {
        Console.WriteLine("Window finalized");
    }
}

namespace WeakEvent
{
    class Program
    {
        static void Main(string[] args)
        {
            var button = new Button();
            var window = new Window(button);

            button.Fire();

            Console.WriteLine("destroying window");
            window = null;
                     
            fireGC();

            /*iako smo stavili da je null, window jos uvijek nije unisten jer je button ziv
             * a u njemu je event na koji smo se subscribe 
             * u ovakvim slucajevima, kad nemamo cist unsubscribe koristi se weak event pattern (ne mere u core a mrzi me da pravim)
             * samo iz nugeta uzmemo windowsbase i zovemo umjesto += 
             * weakeventmanager.addhandler(koja klasa ima event, ime eventa, metod koji ga hendluje)
             */

            Console.ReadLine();
        }

        private static void fireGC()
        {
            Console.WriteLine("garbage day");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Console.WriteLine("is done");

        }
    }
}
