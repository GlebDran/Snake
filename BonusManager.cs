using System; // подключаем базовую библиотеку .NET (для Console, Random и др.)
using System.Collections.Generic; // для использования списков List<>

namespace Snake // пространство имён игры, чтобы все классы были объединены логически
{
    public class BonusManager // класс, управляющий бонусами в игре
    {
        private int mapWidth;  // ширина карты (поля)
        private int mapHeight; // высота карты
        private List<Point> activeBonuses; // список всех активных бонусов на экране
        private Random rand = new Random(); // генератор случайных чисел для размещения бонусов

        // Конструктор: задаёт размеры карты и создаёт пустой список бонусов
        public BonusManager(int width, int height)
        {
            mapWidth = width;              // сохраняем ширину карты
            mapHeight = height;            // сохраняем высоту карты
            activeBonuses = new List<Point>(); // создаём пустой список бонусов
        }

        // Метод для создания нового бонуса (ускорение '$' или заморозка '*')
        public Point CreateBonus(char type)
        {
            // Генерируем случайные координаты для бонуса в пределах карты (не у стен)
            int x = rand.Next(2, mapWidth - 2);
            int y = rand.Next(2, mapHeight - 2);

            // Определяем цвет в зависимости от типа бонуса
            ConsoleColor color = type == '$' ? ConsoleColor.Cyan : ConsoleColor.Magenta;

            // Создаём новый объект Point с координатами и символом
            Point bonus = new Point(x, y, type);

            // Отображаем бонус на экране с нужным цветом
            bonus.Draw(color);

            // Добавляем его в список активных бонусов
            activeBonuses.Add(bonus);

            // Возвращаем созданный бонус
            return bonus;
        }

        // Получаем текущий список активных бонусов
        public List<Point> GetBonuses()
        {
            return activeBonuses;
        }

        // Удаление использованного бонуса с экрана и из списка
        public void RemoveBonus(Point p)
        {
            p.Clear(); // убираем символ с экрана
            activeBonuses.Remove(p); // удаляем из списка
        }

        // Проверяем: не съела ли змейка один из бонусов
        public Point CheckBonusEaten(Snake snake)
        {
            foreach (var bonus in activeBonuses) // перебираем все активные бонусы
            {
                // Если следующая точка головы змейки совпадает с координатами бонуса
                if (snake.GetNextPoint().IsHit(bonus))
                    return bonus; // возвращаем бонус, который съеден
            }
            return null; // если не съела — возвращаем null
        }

        // Полная очистка всех бонусов (например, при перезапуске игры)
        public void ClearAll()
        {
            foreach (var b in activeBonuses) // очищаем каждый бонус с экрана
                b.Clear();
            activeBonuses.Clear(); // очищаем список
        }
    }
}
