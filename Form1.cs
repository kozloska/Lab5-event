using System;
using System.Diagnostics.Metrics;
using WinFormsApp1.Objects;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        MyRectangle myRect;
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        Circle circle1;
        Circle circle2;
        RedCircle redCircle;
        int count = 0;

        public Form1()
        {
            InitializeComponent();

            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);
            player.OnOverlap += (player, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;
            };

            // добавил реакцию на пересечение с маркером
            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };
            player.OnCircleOverlap += (circle) =>
            {
                ZeroCircle(circle);
                count++;
                Counttxt.Text = $"Счёт: {count}";
            };
            player.OnRedCircleOverlap += (Redcircle) =>
            {
                ZeroRedCircle(Redcircle);
                count--;
                Counttxt.Text = $"Счёт: {count}";
            };
            redCircle = new RedCircle(new Random().Next(30, pbMain.Width - 30), new Random().Next(30, pbMain.Height - 30), 0, 5);
            objects.Add(redCircle);


            circle1 = new Circle(new Random().Next(30, pbMain.Width - 30), new Random().Next(30, pbMain.Height - 30), 0, new Random().Next(15, 70), new Random().Next(100, 500));
            circle2 = new Circle(new Random().Next(30, pbMain.Width - 30), new Random().Next(30, pbMain.Height - 30), 0, new Random().Next(15, 70), new Random().Next(100, 500));

           

            objects.Add(player);
            objects.Add(circle1);
            objects.Add(circle2);
            objects.Add(redCircle);
            objects.Add(new MyRectangle(50, 50, 0));
            objects.Add(new MyRectangle(100, 100, 45));
        }
        private void ZeroCircle(Circle circle)
        {
            circle.X = new Random().Next(30, pbMain.Width - 30);
            circle.Y = new Random().Next(30, pbMain.Height - 30);
            circle.radius = new Random().Next(15, 70);
            circle.timer = new Random().Next(100, 500);
        }
        private void ZeroRedCircle(RedCircle circle)
        {
            circle.X = new Random().Next(30, pbMain.Width - 30);
            circle.Y = new Random().Next(30, pbMain.Height - 30);
            circle.radius = 5;
        }
        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.Clear(Color.White);

            updatePlayer();//перерасчёт игрока 

            foreach (var obj in objects.ToList())//пересчитываем пересечения
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj); // то есть игрок пересекся с объектом
                    obj.Overlap(player); // и объект пересекся с игроком

                }
            }
            // рендерим объекты
            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }

        private void updatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;
                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                // по сути мы теперь используем вектор dx, dy
                // как вектор ускорения, точнее даже вектор притяжения
                // который притягивает игрока к маркеру
                // 0.5 просто коэффициент который подобрал на глаз
                // и который дает естественное ощущение движения
                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                // расчитываем угол поворота игрока 
                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }

            // тормозящий момент,
            // нужен чтобы, когда игрок достигнет маркера произошло постепенное замедление
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            // пересчет позиция игрока с помощью вектора скорости
            player.X += player.vX;
            player.Y += player.vY;
        }
        private void GreenCircle()
        {
            circle1.timer--;
            circle2.timer--;
            if (circle1.timer < 0)
            {
                ZeroCircle(circle1);
            }
            else if (circle2.timer < 0)
            {
                ZeroCircle(circle2);
            }
            /* circle1.timer--;
             circle2.timer--;

             /*float d = (float)1 / circle1.timer;
             circle1.radius -= circle1.radius * d;
             d = (float)1 / circle2.timer;
             circle2.radius -= circle2.radius * d;

             if (circle1.timer < 0)
                 ZeroCircle(circle1);
             else if (circle2.timer < 0)
                 ZeroCircle(circle2);*/
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            redCircle.radius = redCircle.radius + 1;

            GreenCircle();

            // запрашиваем обновление pbMain
            // это вызовет метод pbMain_Paint по новой
            pbMain.Invalidate();
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            // тут добавил создание маркера по клику если он еще не создан
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker); // и главное не забыть пололжить в objects
            }

            // а это так и остается
            marker.X = e.X;
            marker.Y = e.Y;
        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}