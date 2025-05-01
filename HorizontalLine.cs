using System.Collections.Generic;

namespace Snake
{
    // Класс для горизонтальной линии — например, верхняя и нижняя границы
    public class HorizontalLine : Figure
    {
        public HorizontalLine(int xLeft, int xRight, int y, char sym)
        {
            pList = new List<Point>();

            // Проходим по оси X и добавляем точки с одинаковой Y-координатой
            for (int x = xLeft; x <= xRight; x++)
            {
                Point p = new Point(x, y, sym);
                pList.Add(p);
            }
        }
    }
}
