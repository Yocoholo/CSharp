using System.Drawing;

namespace wavy
{
    class Point
    {
        public float x, y;
        public float val;
        Pen pen;
        SolidBrush brush;
        int radius = 5;
        public Point(float _x, float _y)
        {
            x = _x - (radius / 2);
            y = _y - (radius / 2);
        }

        public void Draw(Graphics gui)
        {
            gui.DrawEllipse(pen, x, y, radius, radius);
            gui.FillEllipse(brush, x, y, radius, radius);
        }

        public void Value(float value)
        {
            val = (value+1)/2;
            int brightness = (int)(256 * val);
            pen = new Pen(Color.FromArgb(brightness, brightness, brightness));
            brush = new SolidBrush(Color.FromArgb(brightness, brightness, brightness));
        }

        public int State(int x) => val > 0.5 ? 1 * x : 0;
    }
}
