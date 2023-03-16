using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace wavy
{
    public partial class Form1 : Form
    {
        float pointRow = 267;
        float pointCol = 100;
        Point[,] points;
        List<Line> lines;
        Random r = new Random();
        Pen linePen;
        OpenSimplexNoise noise;
        float scale =1;
        float increment = 0.05f;
        float incrementZ = 0.01f;
        float zOff = 0.1f;
        System.Timers.Timer timer;

        public Form1()
        {
            InitializeComponent();
            setUp();

        }

        private void setUp()
        {
            Size = Screen.PrimaryScreen.WorkingArea.Size;
            points = new Point[(int)pointRow + 1, (int)pointCol + 1];
            linePen = new Pen(Color.White, 1);
            noise = new OpenSimplexNoise();
            float iOff = (Size.Width / pointRow) / scale;
            float jOff = (Size.Height / pointCol) / scale;
            DoubleBuffered = true;
            timer = new System.Timers.Timer();
            timer.Interval = 100;
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Enabled = true;

            for (int i = 0; i <= pointRow; i++)
            {
                for (int j = 0; j <= pointCol; j++)
                {
                    points[i, j] = new Point(i * iOff, j * jOff);
                }
            }
            pointVal();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            pointVal();
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            lines = new List<Line>();

            for (int i = 0; i < pointRow; i++)
            {
                for (int j = 0; j < pointCol; j++)
                {
                    int state = points[i, j].State(1) + points[i + 1, j].State(2) + points[i + 1, j + 1].State(4) + points[i, j + 1].State(8);
                    switch (state)
                    {
                        case 1:
                        case 14:
                            lines.Add(new Line(linePen,
                                points[i, j].x,
                                MidPointY(points[i, j], points[i, j + 1]),
                                MidPointX(points[i, j], points[i + 1, j]),
                                points[i, j].y));
                            break;
                        case 2:
                        case 13:
                            lines.Add(new Line(linePen,
                                MidPointX(points[i, j], points[i + 1, j]),
                                points[i, j].y,
                                points[i + 1, j].x,
                                MidPointY(points[i + 1, j], points[i + 1, j + 1])));
                            break;
                        case 4:
                        case 11:
                            lines.Add(new Line(linePen,
                                MidPointX(points[i, j + 1], points[i + 1, j + 1]),
                                points[i, j + 1].y,
                                points[i + 1, j].x,
                                MidPointY(points[i + 1, j], points[i + 1, j + 1])));
                            break;
                        case 8:
                        case 7:
                            lines.Add(new Line(linePen,
                                points[i, j].x,
                                MidPointY(points[i, j], points[i, j + 1]),
                                MidPointX(points[i, j + 1], points[i + 1, j + 1]),
                                points[i, j + 1].y));
                            break;
                        case 3:
                        case 12:
                            lines.Add(new Line(linePen,
                                points[i, j].x,
                                MidPointY(points[i, j], points[i, j + 1]),
                                points[i + 1, j].x,
                                MidPointY(points[i + 1, j], points[i + 1, j + 1])));
                            break;
                        case 6:
                        case 9:
                            lines.Add(new Line(linePen,
                                MidPointX(points[i, j], points[i + 1, j]),
                                points[i, j].y,
                                MidPointX(points[i, j + 1], points[i + 1, j + 1]),
                                points[i, j + 1].y));
                            break;
                        case 5:
                            lines.Add(new Line(linePen,
                                MidPointX(points[i, j], points[i + 1, j]),
                                points[i, j].y,
                                points[i + 1, j].x,
                                MidPointY(points[i + 1, j], points[i + 1, j + 1])));
                            lines.Add(new Line(linePen,
                                points[i, j].x,
                                MidPointY(points[i, j], points[i, j + 1]),
                                MidPointX(points[i, j + 1], points[i + 1, j + 1]),
                                points[i, j + 1].y));
                            break;
                        case 10:
                            lines.Add(new Line(linePen,
                                points[i, j].x,
                                MidPointY(points[i, j], points[i, j + 1]),
                                MidPointX(points[i, j], points[i + 1, j]),
                                points[i, j].y));
                            lines.Add(new Line(linePen,
                                MidPointX(points[i, j + 1], points[i + 1, j + 1]),
                                points[i, j + 1].y,
                                points[i + 1, j].x,
                                MidPointY(points[i + 1, j], points[i + 1, j + 1])));
                            break;
                    }
                }
            }

            Graphics gui = e.Graphics;
            foreach (Point point in points)
            {
                point.Draw(gui);
            }
            foreach (Line line in lines)
            {
                line.Draw(gui);
            }
            zOff += incrementZ;

        }

        private void pointVal()
        {
            float xOff = 0;
            for (int i = 0; i <= pointRow; i++)
            {
                float yOff = 0;
                for (int j = 0; j <= pointCol; j++)
                {
                    float value = (float)noise.Evaluate(xOff, yOff, zOff);
                    points[i, j].Value(value);
                    yOff += increment;
                }
                xOff += increment;
            }
            zOff += incrementZ;
        }

        private float MidPointX(Point pt1, Point pt2)
        {
            float aOff = pt1.val / (pt1.val + pt2.val);
            return aOff * pt1.x + (1 - aOff) * pt2.x;
            //return (pt1.x + pt2.x)/2;
        }
        private float MidPointY(Point pt1, Point pt2)
        {
            float aOff = pt1.val / (pt1.val + pt2.val);
            return aOff * pt1.y + (1 - aOff) * pt2.y;
            //return (pt1.y + pt2.y) / 2;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.W)
            {
                Close();
            }
            else
            {
                pointVal();
                Invalidate();
            }
        }
    }
}
