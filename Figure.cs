using System;
using System.Collections.Generic;

namespace Snake
{
    // Базовый класс для всех фигур (змейка, стены и т.п.)
    public class Figure
    {
        // Инициализируем pList прямо при объявлении — это убирает ошибку CS8618
        protected List<Point> pList = new List<Point>();

        // Метод отрисовки фигуры (всех точек)
        public void Draw()
        {
            foreach (Point p in pList)
            {
                p.Draw();
            }
        }

        // Проверка: пересекается ли с указанной точкой
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
