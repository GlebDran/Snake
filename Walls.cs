using System;

namespace Snake
{
    // Класс Walls создаёт границы игрового поля из линий
    public class Walls
    {
        private List<Figure> wallList;

        public Walls(int width, int height)
        {
            wallList = new List<Figure>();

            // Горизонтальные стены: сверху и снизу
            HorizontalLine topLine = new HorizontalLine(0, width - 1, 0, '+');
            HorizontalLine bottomLine = new HorizontalLine(0, width - 1, height - 1, '+');

            // Вертикальные стены: слева и справа
            VerticalLine leftLine = new VerticalLine(0, height - 1, 0, '+');
            VerticalLine rightLine = new VerticalLine(0, height - 1, width - 1, '+');

            wallList.Add(topLine);
            wallList.Add(bottomLine);
            wallList.Add(leftLine);
            wallList.Add(rightLine);
        }

        // Рисуем все стены
        public void Draw()
        {
            foreach (var wall in wallList)
            {
                wall.Draw();
            }
        }

        // Проверка: врезалась ли змейка в стену
        public bool IsHit(Figure figure)
        {
            foreach (var wall in wallList)
            {
                if (wall.IsHit(figure))
                    return true;
            }
            return false;
        }
    }
}
