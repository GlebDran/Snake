using System;                     // стандартная библиотека: для работы с консолью, строками, клавишами
using System.IO;                  // для чтения/записи в файл (сохранение очков)
using System.Threading;           // для пауз в игре (Thread.Sleep)
using System.Collections.Generic; // для использования списков (List)

namespace Snake // пространство имён проекта
{
    class Program
    {
        static int speed = 150;        // скорость движения змейки (мс)
        static int level = 1;          // текущий уровень
        static int bonusEvery = 5;     // каждые 5 очков появляется новая стена

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // добавлено: поддержка кириллицы в консоли

            while (true)
            {
                RunGame(); // запускаем игру — после Game Over снова сюда
            }
        }

        // Основной игровой процесс
        static void RunGame()
        {
            Console.Title = "Гадюка от Глеба";       // название окна
            Console.CursorVisible = false;           // отключаем мигающий курсор

            int selectedSpeed = ShowMainMenu();      // показываем меню и выбираем сложность
            if (selectedSpeed == -1) Environment.Exit(0); // выход из игры при Esc
            speed = selectedSpeed;

            // Ввод имени игрока
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(20, 10);
            Console.Write("Введите ваше имя: ");
            Console.ResetColor();
            Console.SetCursorPosition(42, 10);
            string playerName = Console.ReadLine();

            // Обработка имени: убираем пробелы, ограничиваем длину
            playerName = playerName.Trim();
            if (string.IsNullOrWhiteSpace(playerName))
                playerName = "Безымянный";
            if (playerName.Length > 20)
                playerName = playerName.Substring(0, 20);

            // Звуки при старте
            Console.Clear();
            Console.Beep(1000, 100);
            Console.Beep(1200, 100);

            int width = 80;
            int height = 25;

            // Создание стен
            Walls walls = new Walls(width, height);
            walls.Draw();

            // Создание змейки
            Point startPoint = new Point(4, 5, '■');
            Snake snake = new Snake(startPoint, 5, Direction.RIGHT);
            snake.Draw();

            // Создание еды
            FoodCreator foodCreator = new FoodCreator(width, height, '$');
            Point food = foodCreator.CreateFood();
            food.Draw(ConsoleColor.Yellow);

            // Создание менеджера бонусов
            BonusManager bonusManager = new BonusManager(width, height);

            int score = 0;
            level = 1;
            ShowScore(score, level);

            bool isPaused = false;

            // Главный игровой цикл
            while (true)
            {
                if (!isPaused)
                {
                    // Проверка проигрыша
                    if (walls.IsHit(snake) || snake.IsHit(snake))
                    {
                        Console.SetCursorPosition(30, 12);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" GAME OVER ");
                        Console.ResetColor();

                        Console.Beep(300, 300);
                        Console.Beep(200, 400);

                        SavePlayerScore(playerName, score);

                        Console.SetCursorPosition(30, 14);
                        Console.WriteLine($"Ваш счёт: {score}");
                        Console.SetCursorPosition(30, 16);
                        Console.WriteLine("Результаты всех игроков:");
                        ShowAllScores();

                        Console.SetCursorPosition(30, 20);
                        Console.WriteLine("Нажмите любую клавишу для возврата в меню...");
                        Console.ReadKey();
                        return; // выходим из RunGame() → обратно в Main()
                    }

                    // Съела ли змейка еду
                    if (snake.Eat(food))
                    {
                        if (food.sym == '$')
                        {
                            Console.Beep(1500, 80);
                            speed = Math.Max(50, speed - 20);
                        }
                        else if (food.sym == '*')
                        {
                            Console.Beep(500, 200);
                            speed += 50;
                        }
                        else
                        {
                            Console.Beep(1400, 80);
                            score++;
                        }

                        // Новый уровень каждые 5 очков
                        if (score > 0 && score % bonusEvery == 0)
                        {
                            level++;
                            walls.AddRandomWall();
                            Console.Beep(1000, 100);
                            Console.Beep(1200, 100);
                        }

                        // Бонус каждые 4 очка
                        if (score % 4 == 0)
                        {
                            char bonusType = (score % 8 == 0) ? '*' : '$';
                            bonusManager.CreateBonus(bonusType);
                        }

                        food = foodCreator.CreateFood();
                        food.Draw(ConsoleColor.Yellow);
                        ShowScore(score, level);
                    }
                    else
                    {
                        snake.Move(); // обычный ход
                    }

                    // Проверка съеденного бонуса
                    Point eatenBonus = bonusManager.CheckBonusEaten(snake);
                    if (eatenBonus != null)
                    {
                        if (eatenBonus.sym == '$') speed = Math.Max(50, speed - 20);
                        if (eatenBonus.sym == '*') speed += 50;
                        bonusManager.RemoveBonus(eatenBonus);
                    }
                }

                // Управление с клавиатуры
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.P)
                    {
                        isPaused = !isPaused;
                        Console.SetCursorPosition(30, 1);
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write(isPaused ? "[ ПАУЗА ]" : "          ");
                        Console.ResetColor();
                    }
                    else
                    {
                        snake.HandleKey(key.Key);
                    }
                }

                Thread.Sleep(speed); // пауза между шагами
            }
        }

        // Меню выбора сложности
        static int ShowMainMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(25, 8);
            Console.WriteLine("███████████████████████████");
            Console.SetCursorPosition(25, 9);
            Console.WriteLine("█      ГАДЮКА от ГЛЕБА     █");
            Console.SetCursorPosition(25, 11);
            Console.WriteLine("█   Выберите сложность:    █");
            Console.SetCursorPosition(25, 12);
            Console.WriteLine("█   1 - Медленно           █");
            Console.SetCursorPosition(25, 13);
            Console.WriteLine("█   2 - Средне             █");
            Console.SetCursorPosition(25, 14);
            Console.WriteLine("█   3 - Быстро             █");
            Console.SetCursorPosition(25, 15);
            Console.WriteLine("█   Esc - Выход из игры    █");
            Console.SetCursorPosition(25, 16);
            Console.WriteLine("███████████████████████████");
            Console.ResetColor();

            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.D1) return 300;
                if (key == ConsoleKey.D2) return 150;
                if (key == ConsoleKey.D3) return 80;
                if (key == ConsoleKey.Escape) return -1;
            }
        }

        // Показ текущего счёта и уровня
        static void ShowScore(int score, int level = 1)
        {
            Console.SetCursorPosition(2, 0);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Очки: {score}  Уровень: {level}  Скорость: {speed}мс   ");
            Console.ResetColor();
        }

        // Сохранение очков игрока
        static void SavePlayerScore(string name, int score)
        {
            string line = $"{name}: {score}";
            File.AppendAllLines("scores.txt", new[] { line });
        }

        // Вывод всех очков из файла
        static void ShowAllScores()
        {
            string path = "scores.txt";
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line) && line.Contains(":"))
                    {
                        Console.WriteLine("  " + line);
                    }
                }
            }
            else
            {
                Console.WriteLine("  Нет сохранённых результатов.");
            }
        }
    }
}
