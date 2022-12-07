using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Objects
{
    class RedCircle : BaseObject // наследуем BaseObject
    {
        // создаем конструктор с тем же набором параметров что и в BaseObject
        // base(x, y, angle) -- вызывает конструктор родительского класса
        public float radius;
        public RedCircle(float x, float y, float angle, float radius) : base(x, y, angle)
        {
            this.radius = radius;
        }
        public override void Render(Graphics g)
        {
            Color c = Color.FromArgb(20, 255, 0, 0);

            g.FillEllipse(new SolidBrush(c), -radius / 2, -radius / 2, radius, radius);
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-radius / 2, -radius / 2, radius, radius);
            return path;
        }
    }
}
