using System;
using System.Collections.Generic;

namespace Snake
{
    public class Walls
    {
        private List<Figure> wallList;

        private int mapWidth;
        private int mapHeight;
        private Random rand = new Random();

        public Walls(int width, int height)
        {
            mapWidth = width;
            mapHeight = height;
            wallList = new List<Figure>();

            // Основная рамка
            wallList.Add(new HorizontalLine(0, width - 1, 0, '+'));
            wallList.Add(new HorizontalLine(0, width - 1, height - 1, '+'));
            wallList.Add(new VerticalLine(0, height - 1, 0, '+'));
            wallList.Add(new VerticalLine(0, height - 1, width - 1, '+'));
        }

        // Отрисовка всех стен
        public void Draw()
        {
            foreach (var wall in wallList)
            {
                wall.Draw();
            }
        }

        // Проверка на столкновение со стеной
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

        // Добавление случайной стены при переходе на новый уровень
        public void AddRandomWall()
        {
            int length = rand.Next(4, 10);
            int x = rand.Next(5, mapWidth - 10);
            int y = rand.Next(3, mapHeight - 3);

            Figure newWall;

            if (rand.Next(2) == 0)
                newWall = new HorizontalLine(x, x + length, y, '#');
            else
                newWall = new VerticalLine(y, y + length, x, '#');

            wallList.Add(newWall);
            newWall.Draw();
        }
    }
}
