using System;
using System.Threading;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            // Размеры карты
            int width = 80;
            int height = 25;

            // 1. Стены
            Walls walls = new Walls(width, height);
            walls.Draw();

            // 2. Змейка
            Point startPoint = new Point(4, 5, '*');
            Snake snake = new Snake(startPoint, 5, Direction.RIGHT);
            snake.Draw();

            // 3. Генератор еды
            FoodCreator foodCreator = new FoodCreator(width, height, '$');
            Point food = foodCreator.CreateFood();
            food.Draw();

            // 4. Главный цикл игры
            while (true)
            {
                // 4.1 Проверка на проигрыш
                if (walls.IsHit(snake) || snake.IsHit(snake))
                {
                    Console.SetCursorPosition(35, 12);
                    Console.WriteLine("GAME OVER");
                    break;
                }

                // 4.2 Проверка на поедание еды
                if (snake.Eat(food))
                {
                    food = foodCreator.CreateFood();
                    food.Draw();
                }
                else
                {
                    snake.Move();
                }

                // 4.3 Управление с клавиатуры
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    snake.HandleKey(key.Key);
                }

                Thread.Sleep(100); // Пауза между шагами
            }

            Console.ReadLine(); // Чтобы окно не закрылось
        }
    }
}
