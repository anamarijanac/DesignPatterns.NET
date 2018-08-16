using Autofac;
using System;

namespace Bridge
{
    public interface IRenderer
    {
        void RenderCircle(float radius);
    }

    public class VectorRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            Console.WriteLine($"Drawing circle of radius {radius}.");
        }
    }

    public class RasterRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            Console.WriteLine($"Drawing pixels for circle of radius {radius}.");
        }
    }

    //umjesto da zadajemo u shape kako ce se crtati pravimo most za odgovarajucu klasu koji crta tj Renderer
    public abstract class Shape
    {
        //ovo je bridge -most 
        protected IRenderer renderer;

        protected Shape(IRenderer renderer)
        {
            this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }

        public abstract void Draw();
        public abstract void Resize(float factor);

    }

    public class Circle : Shape
    {
        private float radius;

        public Circle(IRenderer renderer, float radius) : base(renderer)
        {
            this.radius = radius;
        }

        public override void Draw()
        {
            renderer.RenderCircle(radius);
        }

        public override void Resize(float factor)
        {
            radius *= factor;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //VectorRenderer renderer = new VectorRenderer();
            RasterRenderer renderer = new RasterRenderer();
            var circle = new Circle(renderer, 3);

            circle.Draw();
            circle.Resize(2);
            circle.Draw();

            //nacin sa Dependency Injection jer "je zamorno rucno upisivati renderere" ako ti npr treba 100 krugova sa istim rendererom
            //iz Nugeta nam treba autofac
            var cb = new ContainerBuilder();

            //registrujemo da se automatski kad neki metod trazi IRenderer ubacuje instanca VectorRenderera
            cb.RegisterType<VectorRenderer>().As<IRenderer>().SingleInstance(); // ako su svi isti mozemo stavit da bude singleton

            //registrujemo kako se pravi krug
            cb.Register((c, p) => new Circle(c.Resolve<IRenderer>(), p.Positional<float>(0)));

            //koristimo cb
            using (var c = cb.Build())
            {
                var circleDI = c.Resolve<Circle>(new PositionalParameter(0, 5.0f)); //mora 5.0f ne moze 5
                circleDI.Draw();
                circleDI.Resize(2);
                circleDI.Draw();
            }

            Console.ReadLine();
        }
    }
}
