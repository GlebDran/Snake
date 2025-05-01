using System;
using System.Collections.Generic;

namespace Snake
{
    public class Figure
    {
        protected List<Point> pList = new List<Point>();

        public void Draw()
        {
            foreach (Point p in pList)
            {
                p.Draw();
            }
        }

        public bool IsHit(Point point)
        {
            foreach (Point p in pList)
            {
                if (p.IsHit(point))
                    return true;
            }
            return false;
        }
    }
}
