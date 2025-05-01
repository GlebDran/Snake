using System;
using System.Collections.Generic;

namespace Snake
{
    public class Walls
    {
        private List<Figure> wallList;

        public Walls(int width, int height)
        {
            wallList = new List<Figure>();

            wallList.Add(new HorizontalLine(0, width - 1, 0, '+'));
            wallList.Add(new HorizontalLine(0, width - 1, height - 1, '+'));
            wallList.Add(new VerticalLine(0, height - 1, 0, '+'));
            wallList.Add(new VerticalLine(0, height - 1, width - 1, '+'));
        }

        public void Draw()
        {
            foreach (var wall in wallList)
            {
                wall.Draw();
            }
        }

        public bool IsHit(Snake snake)
        {
            Point next = snake.GetNextPoint();
            foreach (var wall in wallList)
            {
                if (wall.IsHit(next))
                    return true;
            }
            return false;
        }
    }
}
