using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing.Drawing2D;


namespace rain
{
    //public GraphicsPath drop { get; set; }
    class Animator
    {
        private Graphics mainG;
        private int width, heigth;
        private List<Drop> drops = new List<Drop>();
        private Thread t;
        private bool stop = false;
        private BufferedGraphics bg;

        
        public Animator(Graphics g, Rectangle r)
        {
            Update(g, r);
        }
        public void Update(Graphics g, Rectangle r)
        {
            mainG = g;
            width = r.Width;
            heigth = r.Height;
            bg = BufferedGraphicsManager.Current.Allocate(mainG, new Rectangle(0, 0, width, heigth));
            Monitor.Enter(drops);
            foreach (var d in drops)
            {
                d.Update(r);
            }
            Monitor.Exit(drops);
            //Monitor.Enter(drops);//параметр ссылочный тип.
        }
        private void Animate()
        {
            while (!stop)
            {
                
                Graphics g = bg.Graphics;
                g.Clear(Color.FromArgb(192,255,255));
                Monitor.Enter(drops);
                foreach (var d in drops)
                {
                    var drop = new GraphicsPath();
                    drop.StartFigure();
                    drop.AddLine(d.X, d.Y, d.X + 5, d.Y + 15);
                    drop.AddArc(d.X - 5, d.Y + 15, 10, 5, 0, 180);
                    drop.AddLine(d.X - 5,d.Y + 15, d.X, d.Y);
                    drop.CloseFigure();
                    Brush br = new SolidBrush(Color.Blue);
                    g.FillPath(br, drop);
                    Pen p = new Pen(Color.Blue, 2);
                    g.DrawPath(p, drop);

                }
                Monitor.Exit(drops);
                try
                {
                    bg.Render();
                }
                catch (Exception e) { }
                Thread.Sleep(30);
            }
        }
        public void Start()
        {
            if (t == null || !t.IsAlive)
            {
                ThreadStart th = new ThreadStart(Animate);
                t = new Thread(th);
                t.Start();
            }
            var rect = new Rectangle(0, 0, width, heigth);
          
            Drop d = new Drop(rect);
            d.Start();
            Monitor.Enter(drops);
            drops.Add(d);
            Monitor.Exit(drops);

        }
        public void Stop()
        {
            stop = true;
            Monitor.Enter(drops);
            foreach (var d in drops)
            {
                d.Stop();
            }
            drops.Clear();
            Monitor.Exit(drops);
        }
        public void Wind()
        {
            Monitor.Enter(drops);
            foreach (var d in drops)
            {
                d.Wind();
            }
            
            Monitor.Exit(drops);
        }

    }
}
