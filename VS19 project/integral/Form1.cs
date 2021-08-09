using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace integral
{
    public partial class integral : Form
    {
        int W, H;
        int scaleX;
        int scaleY;
        point zero;
        Graphics g;
        Graphics g1;
        Graphics g2;
        Graphics g3;
        Pen iPen;
        Pen lPen;
        Pen fPen;
        int iT, lT, fT;
        string lbl2 = "k=   b= \nx=   y=";
        List<point> a = new List<point>();
        bool mousePressed = false;
        bool drawen = false;
        bool op = false;
        bool needUpdZero = false;
        bool needUpdSegX = false;
        bool needUpdSegY = false;
        bool needGetConst = false;
        bool fflag = false;
        bool drawingFunc = false;
        bool drawenFunc = false;
        Point cnst;
        List<Label> l1, l2, l3, l4;
        public integral()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            Size resolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
            pictureBox1.Width = resolution.Width - 250;
            pictureBox1.Height = resolution.Height;
            pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Image bmp = pictureBox1.Image;
            W = pictureBox1.Width;
            H = pictureBox1.Height;
            scaleX = scaleY = 40;
            zero = new point(W / 2, H / 2);
            g = Graphics.FromImage(bmp);
            g1 = Graphics.FromImage(bmp);
            g2 = pictureBox1.CreateGraphics();
            g3 = pictureBox1.CreateGraphics();
            label2.Text = lbl2;
            Point p = label2.Location;
            p.X -= 310;
            label2.Location = p;
            this.Text = "Integral. Яковлев Сергей. integr48@gmail.com";
            int d = 15;
            label3.Location = new Point(W + d, 10);
            trackBar1.Location = new Point(W + d, 40);
            label4.Location = new Point(W + d, 70);
            trackBar2.Location = new Point(W + d, 100);
            label5.Location = new Point(W + d, 130);
            trackBar3.Location = new Point(W + d, 160);
            label1.Location = new Point(W + d, 200);
            label2.Location = new Point(W + d, 230);
            button1.Location = new Point(W + d, 290);
            button2.Location = new Point(W + d, 340);
            button3.Location = new Point(W + d, 490);
            button4.Location = new Point(W + d, 440);
            button5.Location = new Point(W + d, 390);
            checkBox1.Location = new Point(W + d + 2, 540);
            checkBox2.Location = new Point(W + d + 2, 560);
            label6.Location = new Point(W + d - 3, 580);
            label6.Text = "";
            iPen = new Pen(Color.FromArgb(255, 255, 0, 0), 2);
            fPen = new Pen(Color.FromArgb(255, 0, 255, 0), 2);
            lPen = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
        }
        string cast(double x)
        {
            string s = x.ToString();
            int pos = 0;
            while (pos < s.Length && s[pos] != ',') pos++;
            if (pos == s.Length) return s;
            int t = s.Length - 1;
            while (pos + 2 < t)
            {
                t--;
                s = s.Substring(0, s.Length - 1);
            }
            return s;
        }
        bool flag = false;
        void drawCoords()
        {
            if (!flag)
            {
                flag = true;
                string str = "Зажмите левую кнопку мыши для отрисовки графика производной. " +
                    "Нажмите 'Построить' для автоматической отрисовки графика функции, " +
                    "соответствующей данной производной. " +
                    "Чтобы построить касательную к графику функции в точке, дважды кликните в любой точке " +
                    "с данной x-координатой. " +
                    " Нажмите 'Очистить', чтобы очистить поле. Нажмите 'Сохранить', чтобы сохранить изображение на экране. " + 
                    "Нажмите 'Вставить', чтобы вставить скопированное ранее изображение. Нажмите 'Открыть', чтобы вставить " +
                    "изображение с диска. Чтобы выбрать точку (0, 0) на появившемся изображении, кликните по ней. Чтобы выбрать на появившемся изображении единичный отрезок по оси oX или oY, " + 
                    "выберете точку с координатами (1, 0) либо (0, 1) соответственно. Чтобы выбрать начальную точку (x0, C), где x0 равен абсциссе самой левой точки, кликните на любой точке с ординатой C, " + 
                    "при этом должна стоять соответсвующая галочка. Другая галочка 'Сохранить и сразу открыть' позволяет после сохранения изображения на диске атоматически его открыть.";
                MessageBox.Show(str, "Инструкция");
            }
            Brush br = new SolidBrush(Color.Black);
            int score = 0;
            for (int x = (int)zero.x; x < W; x += scaleX)
            {
                if (score < 10)
                {
                    g1.DrawString(score.ToString(), new Font("Segoe UI", 10F, FontStyle.Italic), br, x, (int)zero.y);
                    g2.DrawString(score.ToString(), new Font("Segoe UI", 10F, FontStyle.Italic), br, x, (int)zero.y);
                    score++;
                }
            }
            score = 0;
            for (int x = (int)zero.x; x >= 0; x -= scaleX)
            {
                if (score < 0 && score > -10)
                {
                    g1.DrawString(score.ToString(), new Font("Segoe UI", 10F, FontStyle.Italic), br, x - 12, (int)zero.y);
                    g2.DrawString(score.ToString(), new Font("Segoe UI", 10F, FontStyle.Italic), br, x - 12, (int)zero.y);
                }
                --score;
            }
            score = 0;
            for (int y = (int)zero.y; y < H; y += scaleY)
            {
                if (score < 10)
                {
                    g1.DrawString((-score).ToString(), new Font("Segoe UI", 10F, FontStyle.Italic), br, W / 2, y);
                    g2.DrawString((-score).ToString(), new Font("Segoe UI", 10F, FontStyle.Italic), br, W / 2, y);
                    score++;
                }
            }
            score = 0;
            for (int y = (int)zero.y; y >= 0; y -= scaleY)
            {
                if (score < 10)
                {
                    g1.DrawString(score.ToString(), new Font("Segoe UI", 10F, FontStyle.Italic), br, W / 2, y);
                    g2.DrawString(score.ToString(), new Font("Segoe UI", 10F, FontStyle.Italic), br, W / 2, y);
                    score++;
                }
            }

            g1.DrawString("Y", new Font("Segoe UI", 15F, FontStyle.Italic), br, (int)zero.x + 5, -3);
            g1.DrawString("X", new Font("Segoe UI", 15F, FontStyle.Italic), br, W - 20, (int)zero.y + 5);
            g1.DrawString("^", new Font("Segoe UI", 17F, FontStyle.Italic), br, (int)zero.x - 11, -7);
            g1.DrawString(">", new Font("Segoe UI", 17F, FontStyle.Italic), br, W - 17, (int)zero.y - 18);

            g2.DrawString("Y", new Font("Segoe UI", 15F, FontStyle.Italic), br, (int)zero.x + 5, 49);
            g2.DrawString("X", new Font("Segoe UI", 15F, FontStyle.Italic), br, W - 20, (int)zero.y + 5);
            g2.DrawString("^", new Font("Segoe UI", 17F, FontStyle.Italic), br, (int)zero.x - 11, 41);
            g2.DrawString(">", new Font("Segoe UI", 17F, FontStyle.Italic), br, W - 17, (int)zero.y - 18);

        }
        void drawCW()
        {
            drawen = true;
            g.DrawLine(Pens.Black, 0, (int)zero.y, W, (int)zero.y);
            g.DrawLine(Pens.Black, (int)zero.x, 0, (int)zero.x, H);
            g3.DrawLine(Pens.Black, 0, (int)zero.y, W, (int)zero.y);
            g3.DrawLine(Pens.Black, (int)zero.x, 0, (int)zero.x, H);
            Pen curPen = new Pen(Color.FromArgb(100, 0, 0, 0), 1);
            for (int x = (int)zero.x; x < W; x += scaleX)
            {
                //g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(x, (int)zero.y - 2, 4, 4));
                g.DrawLine(curPen, x, 0, x, H);
                g3.DrawLine(curPen, x, 0, x, H);
            }
            for (int x = (int)zero.x; x >= 0; x -= scaleX)
            {
                //g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(x, (int)zero.y - 2, 4, 4));
                g.DrawLine(curPen, x, 0, x, H);
                g3.DrawLine(curPen, x, 0, x, H);
            }
            for (int y = (int)zero.y; y < H; y += scaleY)
            {
                //g.FillRectangle(new SolidBrush(Color.Black), new Rectangle((int)zero.x - 2, y, 4, 4));
                g.DrawLine(curPen, 0, y, W, y);
                g3.DrawLine(curPen, 0, y, W, y);
            }
            for (int y = (int)zero.y; y >= 0; y -= scaleY)
            {
                //g.FillRectangle(new SolidBrush(Color.Black), new Rectangle((int)zero.x - 2, y, 4, 4));
                g.DrawLine(curPen, 0, y, W, y);
                g3.DrawLine(curPen, 0, y, W, y);
            }
        }

        point convert(Point p)
        {
            point res = new point(p);
            res.y = H - res.y;
            res.y -= zero.y;
            res.x -= zero.x;
            res.y /= scaleY;
            res.x /= scaleX;
            return res;
        }
        Point convert(point p)
        {
            p.x *= scaleX;
            p.y *= scaleY;
            Point res = new Point((int)p.x, (int)p.y);
            res.X += (int)zero.x;
            res.Y += (int)zero.y;
            res.Y = H - res.Y;
            return res;
        }
        private void PictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        public void clear()
        {
            label2.Text = lbl2;
            f.Clear();
            b.Clear();
            a.Clear();
            //pictureBox1.Dispose();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Image bmp = pictureBox1.Image;
            W = pictureBox1.Width;
            H = pictureBox1.Height;
            g = Graphics.FromImage(bmp);
            g1 = Graphics.FromImage(bmp);
            g2 = pictureBox1.CreateGraphics();
            g3 = pictureBox1.CreateGraphics();
            drawen = false;
            op = false;
            zero = new point(W / 2, H / 2);
            scaleX = scaleY = 40;
            needUpdZero = false;
            needUpdSegX = false;
            needUpdSegY = false;
            needGetConst = false;
            fflag = false;
            drawingFunc = false;
            drawenFunc = false;
        }
        List<Point> b = new List<Point>();
        List<point> f = new List<point>();
        public void drawIntegral()
        {
            if (a.Count() == 0) return;
            for (int i = 1; i < a.Count(); i++)
            {
                a[i].y = a[i - 1].y + a[i].y * (a[i].x - a[i - 1].x);            
            }
            int d = 0;
            Point u = convert(a[0]);
            if (fflag)
            {
                d = -u.Y + cnst.Y;
                fflag = false;
            }
            u.Y += d;
            b.Add(u);
            for (int i = 1; i < a.Count(); i++)
            {
                u = convert(a[i]);
                u.Y += d;
                b.Add(u);
                g.DrawLine(iPen, b[i - 1], b[i]);
                g3.DrawLine(iPen, b[i - 1], b[i]);
                //drawBetween(b[i - 1], b[i]);
            }
            a.Clear();
        }

        void drawBetween(Point a, Point b)
        {
            if (b.X == a.X)
            {
                g.DrawLine(iPen, a, b);
                g3.DrawLine(iPen, a, b);
                return;
            }
            int k = (b.Y - a.Y) / (b.X - a.X);
            Point prev = a;
            for (int i = 0; a.X + i <= b.X; i++)
            {
                int Y = a.Y + i * k;
                int X = a.X + i;
                Point p = new Point(X, Y);
                g.DrawLine(iPen, prev, p);
                g3.DrawLine(iPen, prev, p);
                prev = p;
            }
            Console.Write(a.X);
            Console.Write(" ");
            Console.Write(a.Y);
            Console.Write("\n");
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked == false)
                drawIntegral();
            else
            {
                needGetConst = true;
                fflag = true;
                label6.Text = "Выберите константу";
            }
        }

        
        private void Button2_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mousePressed = true;
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (mousePressed && drawingFunc)
                drawenFunc = true;
            mousePressed = false;
        }
        Point prev;
        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            point v = convert(p);
            point t = convert(p);
            label1.Text = cast(t.x) + " " + cast(t.y);
            if (mousePressed && !drawenFunc)
            {
                drawingFunc = true;
                f.Add(v);
                a.Add(t);
                Pen curPen = fPen;
                if (a.Count() == 1)
                {               
                    g.DrawLine(curPen, p, p);
                    g3.DrawLine(curPen, p, p);
                } else
                {
                    g.DrawLine(curPen, prev, p);
                    g3.DrawLine(curPen, prev, p);
                }
                prev = p;
            }
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (!op)
            {
                if (!drawen)
                {
                    drawCW();
                }
                drawCoords();
            }
        }

        void drawLine(point a, point b)
        {
            Pen curPen = lPen;
            Point A = convert(a);
            Point B = convert(b);
            g.DrawLine(curPen, A, B);
            g3.DrawLine(curPen, A, B);
        }

        private void PictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Point p = Cursor.Position;
            if (b.Count() == 0)
            {
                return;
            }
            int i = 0;
            for (int j = 0; j < b.Count(); j++)
            {
                if (Math.Abs(b[j].X - p.X) < Math.Abs(b[i].X - p.X))
                {
                    i = j;
                }
            }
            point c = convert(b[i]);
            double n = f[i].y;
            double m = c.y;
            double A = c.x;
            //y=n*x+m-n*A;
            drawLine(new point(-500, -n * 500 + m - n * A), new point(500, (500 * n + m - n * A)));
            //g.FillEllipse(Brushes.Yellow, b[i].X, b[i].Y - 15, 15, 15);
            label2.Text = "k=" + cast(n) + "  b=" + cast(c.x) + "\n" + 
                "x=" + cast(f[i].x) + "  y=" + cast(c.y);
        }

        

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        string path = "";
        private void Button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = ("Сохранить как...");
            save.Filter = "Image document (*.jpeg) | *.jpeg| All Files (*.*)|*.*";
            save.OverwritePrompt = true;
            if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = save.FileName;
                //pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
                (pictureBox1.Image as Bitmap).Save(save.FileName, System.Drawing.Imaging.ImageFormat.Png);
                if (checkBox1.Checked)
                {
                    Process.Start(path);
                }
            }
            //saving
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (Clipboard.GetImage() == null) return;
            clear();
            Image i = Clipboard.GetImage();
            pictureBox1.Image = new Bitmap(i, W, H);
            op = true;
            needUpdZero = true;
            label6.Text = "Выберите ноль";
            Image bmp = pictureBox1.Image;
            g = Graphics.FromImage(bmp);
            g1 = Graphics.FromImage(bmp);
        }

        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (needGetConst)
            {
                cnst.Y = e.Y;
                needGetConst = false;
                fflag = true;
                label6.Text = "";
                drawIntegral();
                return;
            }
            if (needUpdZero)
            {
                int xxx = (int)e.X;
                int yyy = (int)e.Y;
                zero = new point(xxx, H - yyy - 1);
                needUpdZero = false;
                label6.Text = "Выберите единичный\nотрезок по oX";
                needUpdSegX = true;
                return;
            } 
            if (needUpdSegX)
            {
                needUpdSegX = false;
                needUpdSegY = true;
                int xxx = (int)e.X;
                int yyy = H - (int)e.Y - 1;
                int dx = xxx - (int)zero.x;
                int dy = yyy - (int)zero.y;
                int dist = (int)Math.Sqrt(dx * dx + dy * dy);
                scaleX = dist;
                label6.Text = "Выберите единичный\nотрезок по oY";
                return;
            }
            if (needUpdSegY)
            {
                needUpdSegY = false;
                int xxx = (int)e.X;
                int yyy = H - (int)e.Y - 1;
                int dx = xxx - (int)zero.x;
                int dy = yyy - (int)zero.y;
                int dist = (int)Math.Sqrt(dx * dx + dy * dy);
                scaleY = dist;
                label6.Text = "";
                return;
            }
        }

        private void Button5_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //open file
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image i = Image.FromFile(openFileDialog1.FileName);
                clear();
                pictureBox1.Image = new Bitmap(i, W, H);
                op = true;
                needUpdZero = true;
                label6.Text = "Выберите ноль";
                Image bmp = pictureBox1.Image;
                g = Graphics.FromImage(bmp);
                g1 = Graphics.FromImage(bmp);
            }
        }

        private void Label3_MouseClick(object sender, MouseEventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            iPen.Color = colorDialog1.Color;
            label3.ForeColor = iPen.Color;
        }

        private void Label4_MouseClick(object sender, MouseEventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            fPen.Color = colorDialog1.Color;
            label4.ForeColor = fPen.Color;
        }

        private void Label5_MouseClick(object sender, MouseEventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            lPen.Color = colorDialog1.Color;
            label5.ForeColor = lPen.Color;
        }

        private void TrackBar3_Scroll(object sender, EventArgs e)
        {
            lPen.Width = (float)trackBar3.Value;
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void TrackBar1_Scroll_1(object sender, EventArgs e)
        {
            iPen.Width = (float)trackBar1.Value;
        }

        private void TrackBar2_Scroll_1(object sender, EventArgs e)
        {
            fPen.Width = (float)trackBar2.Value;
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }

        public class point
        {
            public double x, y;
            public point(double _x = 0, double _y = 0)
            {
                x = _x;
                y = _y;
            }
            public point(Point p)
            {
                x = p.X;
                y = p.Y;
            }
        }
    }
}
