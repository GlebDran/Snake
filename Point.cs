using System;

namespace Snake
{
    public class Point
    {
        public int x;
        public int y;
        public char sym;

        public Point() { }

        public Point(int x, int y, char sym)
        {
            this.x = x;
            this.y = y;
            this.sym = sym;
        }

        public void Draw()
        {
            Console.SetCursorPosition(x, y);
            Console.Write(sym);
        }

        public void Draw(ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(sym);
            Console.ResetColor();
        }

        public void Clear()
        {
            sym = ' ';
            Draw();
        }

        public void Move(int offset, Direction direction)
        {
            switch (direction)
            {
                case Direction.RIGHT: x += offset; break;
                case Direction.LEFT: x -= offset; break;
                case Direction.UP: y -= offset; break;
                case Direction.DOWN: y += offset; break;
            }
        }

        public bool IsHit(Point p)
        {
            return p.x == x && p.y == y;
        }
    }
}
