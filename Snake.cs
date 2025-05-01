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

            // Создаём начальное тело змейки
            for (int i = 0; i < length; i++)
            {
                Point p = new Point(tail.x, tail.y, tail.sym);
                p.Move(i, direction); // сдвигаем каждую точку в нужном направлении
                pList.Add(p);
            }
        }

        // Двигаем змейку на 1 шаг вперёд
        public void Move()
        {
            // Удаляем хвостовую точку (затираем её)
            Point tail = pList[0];
            pList.RemoveAt(0);
            tail.Clear();

            // Добавляем новую голову
            Point head = GetNextPoint();
            pList.Add(head);
            head.Draw();
        }

        // Получить новую точку головы
        public Point GetNextPoint()
        {
            Point head = pList[pList.Count - 1];
            Point nextPoint = new Point(head.x, head.y, head.sym);
            nextPoint.Move(1, direction);
            return nextPoint;
        }

        // Управление — смена направления по нажатию клавиши
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

        // Проверка на поедание еды
        public bool Eat(Point food)
        {
            Point head = GetNextPoint();
            if (head.IsHit(food))
            {
                pList.Add(food); // не удаляем хвост — змея становится длиннее
                return true;
            }
            return false;
        }

        // Проверка на самопересечение змейки
        public bool IsHit(Figure figure)
        {
            Point head = pList[pList.Count - 1];
            for (int i = 0; i < pList.Count - 1; i++)
            {
                if (head.IsHit(pList[i]))
                    return true;
            }
            return false;
        }
    }
}
