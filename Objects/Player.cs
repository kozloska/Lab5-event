using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Objects
{
   class Player : BaseObject
   {
        public Action<Marker> OnMarkerOverlap; 
        public Action<Circle> OnCircleOverlap;
        public Action<RedCircle> OnRedCircleOverlap;
        public float vX, vY; //поля под вектор скорости
        public Player(float x, float y, float angle) : base(x, y, angle)
        {

        }//конструктор

        public override void Render(Graphics g)
        {
            g.FillEllipse(
                new SolidBrush(Color.DeepSkyBlue),
                -15, -15,
                30, 30
            );//кружочек с синим фоном

            g.DrawEllipse(
                new Pen(Color.Black, 2),
                -15, -15, 
                30, 30
            );//очерчиваю кружочку рамку

            g.DrawLine(new Pen(Color.Black, 2), 0, 0, 25, 0);//рисую палочку, указывающую направление игрока
        }

        public override GraphicsPath GetGraphicsPath()//добавление круга совпадающего по размеру с кругом в Render
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-15, -15, 30, 30);
            return path;
        }

        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);
            if(obj is Marker)
            {
                OnMarkerOverlap(obj as Marker);
            }
            if (obj is Circle)
            {
                OnCircleOverlap(obj as Circle);
            }
            if (obj is RedCircle)
            {
                OnRedCircleOverlap(obj as RedCircle);
            }
        }
    }
}
