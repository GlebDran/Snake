using System.Collections.Generic;

namespace Snake
{
    // Класс для вертикальной линии — левая и правая границы
    public class VerticalLine : Figure
    {
        public VerticalLine(int yTop, int yBottom, int x, char sym)
        {
            pList = new List<Point>();

            // Проходим по оси Y и добавляем точки с одинаковой X-координатой
            for (int y = yTop; y <= yBottom; y++)
            {
                Point p = new Point(x, y, sym);
                pList.Add(p);
            }
        }
    }
}
