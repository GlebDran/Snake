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

        // Метод рисует символ на позиции (x, y)
        public void Draw()
        {
            Console.SetCursorPosition(x, y);
            Console.Write(sym);
        }

        // Метод стирает символ с экрана (пробелом)
        public void Clear()
        {
            sym = ' ';
            Draw();
        }

        // Копирует точку и смещает её в заданном направлении
        public void Move(int offset, Direction direction)
        {
            switch (direction)
            {
                case Direction.RIGHT:
                    x += offset;
                    break;
                case Direction.LEFT:
                    x -= offset;
                    break;
                case Direction.UP:
                    y -= offset;
                    break;
                case Direction.DOWN:
                    y += offset;
                    break;
            }
        }

        // Проверка на совпадение координат с другой точкой
        public bool IsHit(Point p)
        {
            return p.x == x && p.y == y;
        }
    }
}
