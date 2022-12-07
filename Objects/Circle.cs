using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Objects
{
    class Circle : BaseObject // наследуем BaseObject
    {
        // создаем конструктор с тем же набором параметров что и в BaseObject
        // base(x, y, angle) -- вызывает конструктор родительского класса
        public float radius;
        public int timer;
        public Circle(float x, float y, float angle, float radius, int timer) : base(x, y, angle)
        {
            this.radius = radius;
            this.timer = timer;
        }
        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.SeaGreen), -radius / 2, -radius / 2, radius, radius);
            g.DrawString(timer.ToString(), new Font("Verdana", 12), new SolidBrush(Color.Black), radius / 3, radius / 3);
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-radius / 2, -radius / 2, radius, radius);
            return path;
        }
    }
}
