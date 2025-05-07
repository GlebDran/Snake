using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Media;  // для SoundPlayer
using System.Text;

namespace Snake
{
    class Program
    {
        static int speed = 150;              // начальная скорость змейки
        static int level = 1;                // текущий уровень
        static int bonusEvery = 5;           // каждые N очков — новый уровень

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8; // для отображения русских символов и символа змейки ☻
            while (true)
            {
                RunGame(); // запускаем игру снова после завершения
            }
        }

        static void RunGame()
        {
            Console.Title = "Гадюка от Глеба";
            Console.CursorVisible = false;

            int selectedSpeed = ShowMainMenu();
            if (selectedSpeed == -1) Environment.Exit(0);
            speed = selectedSpeed;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(20, 10);
            Console.Write("Введите ваше имя: ");
            Console.ResetColor();
            Console.SetCursorPosition(42, 10);
            string playerName = Console.ReadLine();

            // Обработка имени: убираем пробелы и ограничиваем длину
            playerName = playerName.Trim();
            if (string.IsNullOrWhiteSpace(playerName))
                playerName = "Безымянный";
            if (playerName.Length > 20)
                playerName = playerName.Substring(0, 20);

            Console.Clear();
            Console.Beep(1000, 100);
            Console.Beep(1200, 100);

            int width = 80;
            int height = 25;

            Walls walls = new Walls(width, height);
            walls.Draw();

            Point startPoint = new Point(4, 5, '■');
            Snake snake = new Snake(startPoint, 5, Direction.RIGHT);
            snake.Draw();

            FoodCreator foodCreator = new FoodCreator(width, height, '$');
            Point food = foodCreator.CreateFood();
            food.Draw(ConsoleColor.Yellow);

            BonusManager bonusManager = new BonusManager(width, height);

            int score = 0;
            level = 1;
            ShowScore(score, level);

            bool isPaused = false;

            while (true)
            {
                if (!isPaused)
                {
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
                        return;
                    }

                    if (snake.Eat(food))
                    {
                        PlayEatSound(); // проигрываем звук поедания

                        if (food.sym == '$') speed = Math.Max(50, speed - 20);
                        else if (food.sym == '*') speed += 50;
                        else score++;

                        if (score > 0 && score % bonusEvery == 0)
                        {
                            level++;
                            walls.AddRandomWall();
                            Console.Beep(1000, 100);
                            Console.Beep(1200, 100);
                        }

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
                        snake.Move();
                    }

                    Point eatenBonus = bonusManager.CheckBonusEaten(snake);
                    if (eatenBonus != null)
                    {
                        if (eatenBonus.sym == '$') speed = Math.Max(50, speed - 20);
                        if (eatenBonus.sym == '*') speed += 50;
                        bonusManager.RemoveBonus(eatenBonus);
                    }
                }

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

                Thread.Sleep(speed);
            }
        }

        // Проигрывание звука поедания
        static void PlayEatSound()
        {
            try
            {
                using (SoundPlayer player = new SoundPlayer("eat.wav"))
                {
                    player.Play(); // асинхронное воспроизведение
                }
            }
            catch
            {
                // если файла нет или ошибка воспроизведения — ничего не делаем
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

        // Вывод текущего счёта
        static void ShowScore(int score, int level = 1)
        {
            Console.SetCursorPosition(2, 0);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Очки: {score}  Уровень: {level}  Скорость: {speed}мс   ");
            Console.ResetColor();
        }

        // Сохраняем игрока и счёт в файл
        static void SavePlayerScore(string name, int score)
        {
            string line = $"{name}: {score}";
            File.AppendAllLines("scores.txt", new[] { line });
        }

        // Показываем таблицу рекордов
        static void ShowAllScores()
        {
            string path = "scores.txt";
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line) && line.Contains(":"))
                        Console.WriteLine("  " + line);
                }
            }
            else
            {
                Console.WriteLine("  Нет сохранённых результатов.");
            }
        }
    }
}
