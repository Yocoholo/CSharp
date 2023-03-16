using System.Drawing;

namespace wavy
{
    class Line
    {
        Pen LinePen;
        float _x1, _y1, _x2, _y2;

        public Line(Pen line, float x1, float y1, float x2, float y2)
        {
            LinePen = line;
            _x1 = x1;
            _y1 = y1;
            _x2 = x2;
            _y2 = y2;
        }
        public void Draw(Graphics gui)
        {
            gui.DrawLine(LinePen, _x1, _y1, _x2, _y2);
        }
    }
}
