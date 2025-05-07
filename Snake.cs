using System;                    // стандартная библиотека (для ConsoleKey и т.п.)
using System.Collections.Generic; // подключаем список List<>

namespace Snake // пространство имён, объединяющее все классы игры
{
    // Класс змейки — наследует Figure, чтобы использовать pList, Draw(), IsHit()
    public class Snake : Figure
    {
        private Direction direction; // текущее направление движения змейки

        // Конструктор: задаёт начальную позицию хвоста, длину и направление
        public Snake(Point tail, int length, Direction _direction)
        {
            direction = _direction;         // сохраняем направление
            pList = new List<Point>();      // создаём список точек тела змейки

            // Создаём начальное тело змейки (последовательно двигаем точки от хвоста)
            for (int i = 0; i < length; i++)
            {
                Point p = new Point(tail.x, tail.y, '■'); // каждая точка — квадрат '■'
                p.Move(i, direction); // смещаем каждую точку вперёд на i позиций
                pList.Add(p);         // добавляем точку в тело змейки
            }
        }

        // Метод для движения змейки вперёд
        public void Move()
        {
            // Удаляем хвост: стираем самую первую точку (чтобы длина оставалась постоянной)
            Point tail = pList[0];
            pList.RemoveAt(0);
            tail.Clear();

            // Вычисляем следующую точку головы и добавляем в конец списка
            Point head = GetNextPoint();
            pList.Add(head);

            // Отрисовываем тело змейки зелёным
            for (int i = 0; i < pList.Count - 1; i++)
            {
                pList[i].sym = '■'; // тело — зелёные квадраты
                pList[i].Draw(ConsoleColor.Green);
            }

            // Отрисовываем голову другим символом и цветом — для визуального выделения
            head.sym = '☻'; // голова змейки — смайлик
            head.Draw(ConsoleColor.Yellow);
        }

        // Метод для определения следующей точки, куда должна двигаться голова
        public Point GetNextPoint()
        {
            Point head = pList[pList.Count - 1]; // берём текущую голову
            Point nextPoint = new Point(head.x, head.y, head.sym); // копируем координаты
            nextPoint.Move(1, direction); // сдвигаем её на 1 шаг вперёд
            return nextPoint;
        }

        // Метод обработки нажатий клавиш (стрелки): меняем направление
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

        // Метод проверки: съела ли змейка еду
        public bool Eat(Point food)
        {
            Point head = GetNextPoint(); // получаем следующую точку головы
            if (head.IsHit(food))        // если она совпадает с точкой еды
            {
                food.sym = '■';          // превращаем еду в часть тела
                pList.Add(food);         // добавляем как новую "голову"
                return true;             // возвращаем true → еда съедена
            }
            return false; // иначе — не съела
        }

        // Метод проверки столкновения с чем-то (например, стенами или собой)
        public bool IsHit(Figure figure)
        {
            Point head = GetNextPoint();   // получаем следующую позицию головы
            return figure.IsHit(head);     // проверяем — сталкивается ли она с переданной фигурой
        }
    }
}
