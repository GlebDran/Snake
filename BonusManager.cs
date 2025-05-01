using System;
using System.Collections.Generic;

namespace Snake
{
    public class BonusManager
    {
        private int mapWidth;
        private int mapHeight;
        private List<Point> activeBonuses;
        private Random rand = new Random();

        public BonusManager(int width, int height)
        {
            mapWidth = width;
            mapHeight = height;
            activeBonuses = new List<Point>();
        }

        // Создать бонус: символ $ (ускорение) или * (заморозка)
        public Point CreateBonus(char type)
        {
            int x = rand.Next(2, mapWidth - 2);
            int y = rand.Next(2, mapHeight - 2);

            ConsoleColor color = type == '$' ? ConsoleColor.Cyan : ConsoleColor.Magenta;

            Point bonus = new Point(x, y, type);
            bonus.Draw(color);

            activeBonuses.Add(bonus);
            return bonus;
        }

        // Получить список всех бонусов
        public List<Point> GetBonuses()
        {
            return activeBonuses;
        }

        // Удалить использованный бонус
        public void RemoveBonus(Point p)
        {
            p.Clear();
            activeBonuses.Remove(p);
        }

        // Проверка — съела ли змейка бонус
        public Point CheckBonusEaten(Snake snake)
        {
            foreach (var bonus in activeBonuses)
            {
                if (snake.GetNextPoint().IsHit(bonus))
                    return bonus;
            }
            return null;
        }

        // Очистить все бонусы (например, при перезапуске)
        public void ClearAll()
        {
            foreach (var b in activeBonuses)
                b.Clear();
            activeBonuses.Clear();
        }
    }
}
