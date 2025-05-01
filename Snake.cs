using System;
using System.Collections.Generic;

namespace Snake
{
    public class Snake : Figure
    {
        private Direction direction;

        public Snake(Point tail, int length, Direction _direction)
        {
            direction = _direction;
            pList = new List<Point>();

            // Формируем начальное тело змейки
            for (int i = 0; i < length; i++)
            {
                // Используем символ тела: '■'
                Point p = new Point(tail.x, tail.y, '■');
                p.Move(i, direction);
                pList.Add(p);
            }
        }

        // Движение змейки
        public void Move()
        {
            // Удаляем хвост
            Point tail = pList[0];
            pList.RemoveAt(0);
            tail.Clear();

            // Добавляем новую голову
            Point head = GetNextPoint();
            pList.Add(head);

            // Отрисовка тела: зелёным
            for (int i = 0; i < pList.Count - 1; i++)
            {
                pList[i].sym = '■'; // тело
                pList[i].Draw(ConsoleColor.Green);
            }

            // Отрисовка головы: жёлтым другим символом
            head.sym = '☻';
            head.Draw(ConsoleColor.Yellow);
        }

        // Вычисляем следующую точку головы
        public Point GetNextPoint()
        {
            Point head = pList[pList.Count - 1];
            Point nextPoint = new Point(head.x, head.y, head.sym);
            nextPoint.Move(1, direction);
            return nextPoint;
        }

        // Управление стрелками
        public void HandleKey(ConsoleKey key)
        {
            if (key == ConsoleKey.LeftArrow && direction != Direction.RIGHT)
                direction = Direction.LEFT;
            else if (key == ConsoleKey.RightArrow && direction != Direction.LEFT)
                direction = Direction.RIGHT;
            else if (key == ConsoleKey.UpArrow && direction != Direction.DOWN)
                direction = Direction.UP;
            else if (key == ConsoleKey.DownArrow && direction != Direction.UP)
                direction = Direction.DOWN;
        }

        // Проверка поедания еды
        public bool Eat(Point food)
        {
            Point head = GetNextPoint();
            if (head.IsHit(food))
            {
                food.sym = '■'; // добавляем как тело
                pList.Add(food);
                return true;
            }
            return false;
        }

        // Проверка самопересечения
        public bool IsHit(Figure figure)
        {
            Point head = GetNextPoint();
            return figure.IsHit(head);
        }
    }
}
