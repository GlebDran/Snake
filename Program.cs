using System;                     // стандартная библиотека — для работы с консолью, временем и строками
using System.IO;                  // позволяет читать/писать в файлы (счёт игроков)
using System.Threading;           // для паузы между шагами змейки (Thread.Sleep)
using System.Collections.Generic; // используется в бонусах, стенах и списках точек

namespace Snake // пространство имён, объединяющее все игровые классы
{
    class Program
    {
        // Переменные, определяющие скорость игры, уровень, и частоту появления новых стен
        static int speed = 150;         // скорость змейки в миллисекундах
        static int level = 1;           // текущий уровень (начинаем с 1)
        static int bonusEvery = 5;      // каждые 5 очков — новый уровень и новая стена

        static void Main(string[] args)
        {
            // Главный цикл: после окончания игры снова запускает RunGame()
            while (true)
            {
                RunGame(); // одна сессия игры
            }
        }

        // Основная логика запуска и хода игры
        static void RunGame()
        {
            Console.Title = "Гадюка от Глеба";     // заголовок окна консоли
            Console.CursorVisible = false;         // скрываем мигающий курсор

            int selectedSpeed = ShowMainMenu();    // показать меню и выбрать скорость
            if (selectedSpeed == -1) Environment.Exit(0); // если нажали Esc — выходим
            speed = selectedSpeed;

            // Ввод имени игрока
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(20, 10);
            Console.Write("Введите ваше имя: ");
            Console.ResetColor();
            Console.SetCursorPosition(42, 10);
            string playerName = Console.ReadLine();

            // Обработка имени: удаляем пробелы, проверяем пустое, ограничиваем длину
            playerName = playerName.Trim();
            if (string.IsNullOrWhiteSpace(playerName))
                playerName = "Безымянный";
            if (playerName.Length > 20)
                playerName = playerName.Substring(0, 20);

            // Стартовые звуки
            Console.Clear();
            Console.Beep(1000, 100);
            Console.Beep(1200, 100);

            // Размеры карты
            int width = 80;
            int height = 25;

            // Создаём стены и отрисовываем
            Walls walls = new Walls(width, height);
            walls.Draw();

            // Создаём змейку из 5 элементов, направленную вправо
            Point startPoint = new Point(4, 5, '■');
            Snake snake = new Snake(startPoint, 5, Direction.RIGHT);
            snake.Draw();

            // Генератор еды (из символа '$')
            FoodCreator foodCreator = new FoodCreator(width, height, '$');
            Point food = foodCreator.CreateFood();
            food.Draw(ConsoleColor.Yellow); // рисуем еду жёлтым

            // Менеджер бонусов
            BonusManager bonusManager = new BonusManager(width, height);

            int score = 0;
            level = 1;
            ShowScore(score, level); // отрисовываем начальные очки

            bool isPaused = false;

            // Главный цикл игры
            while (true)
            {
                if (!isPaused)
                {
                    // Проверка на проигрыш (стена или сам себя съел)
                    if (walls.IsHit(snake) || snake.IsHit(snake))
                    {
                        // Сообщаем о проигрыше
                        Console.SetCursorPosition(30, 12);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" GAME OVER ");
                        Console.ResetColor();

                        Console.Beep(300, 300); // грустный звук
                        Console.Beep(200, 400);

                        // Сохраняем результат
                        SavePlayerScore(playerName, score);

                        // Печатаем счёт и результаты всех игроков
                        Console.SetCursorPosition(30, 14);
                        Console.WriteLine($"Ваш счёт: {score}");
                        Console.SetCursorPosition(30, 16);
                        Console.WriteLine("Результаты всех игроков:");
                        ShowAllScores();

                        // Пауза перед возвращением в меню
                        Console.SetCursorPosition(30, 20);
                        Console.WriteLine("Нажмите любую клавишу для возврата в меню...");
                        Console.ReadKey();
                        return; // выходим из RunGame(), возвращаемся в Main
                    }

                    // Если змейка съела еду
                    if (snake.Eat(food))
                    {
                        // Если еда — это ускорение
                        if (food.sym == '$')
                        {
                            Console.Beep(1500, 80);
                            speed = Math.Max(50, speed - 20);
                        }
                        // Если еда — это заморозка
                        else if (food.sym == '*')
                        {
                            Console.Beep(500, 200);
                            speed += 50;
                        }
                        else // обычная еда
                        {
                            Console.Beep(1400, 80);
                            score++; // увеличиваем очки
                        }

                        // Каждые 5 очков: новый уровень и новая стена
                        if (score > 0 && score % bonusEvery == 0)
                        {
                            level++;
                            walls.AddRandomWall();
                            Console.Beep(1000, 100);
                            Console.Beep(1200, 100);
                        }

                        // Каждые 4 очка — создаём бонус ($ или *)
                        if (score % 4 == 0)
                        {
                            char bonusType = (score % 8 == 0) ? '*' : '$';
                            bonusManager.CreateBonus(bonusType);
                        }

                        // Создаём новую еду
                        food = foodCreator.CreateFood();
                        food.Draw(ConsoleColor.Yellow);

                        // Обновляем отображение очков и уровня
                        ShowScore(score, level);
                    }
                    else
                    {
                        // Просто двигаем змейку, если не съела
                        snake.Move();
                    }

                    // Проверка на съеденный бонус
                    Point eatenBonus = bonusManager.CheckBonusEaten(snake);
                    if (eatenBonus != null)
                    {
                        if (eatenBonus.sym == '$') speed = Math.Max(50, speed - 20);
                        if (eatenBonus.sym == '*') speed += 50;
                        bonusManager.RemoveBonus(eatenBonus); // удаляем бонус с карты
                    }
                }

                // Обработка клавиш
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.P)
                    {
                        isPaused = !isPaused; // пауза вкл/выкл
                        Console.SetCursorPosition(30, 1);
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write(isPaused ? "[ ПАУЗА ]" : "          ");
                        Console.ResetColor();
                    }
                    else
                    {
                        snake.HandleKey(key.Key); // обработка движения
                    }
                }

                Thread.Sleep(speed); // задержка между шагами
            }
        }

        // Меню запуска игры с выбором скорости
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

        // Показ счёта, уровня и скорости в верхней строке
        static void ShowScore(int score, int level = 1)
        {
            Console.SetCursorPosition(2, 0);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Очки: {score}  Уровень: {level}  Скорость: {speed}мс   ");
            Console.ResetColor();
        }

        // Сохраняем имя и счёт игрока в текстовый файл
        static void SavePlayerScore(string name, int score)
        {
            string line = $"{name}: {score}";
            File.AppendAllLines("scores.txt", new[] { line });
        }

        // Чтение и вывод всех результатов из файла
        static void ShowAllScores()
        {
            string path = "scores.txt";
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                foreach (var line in lines)
                {
                    // Показываем только непустые строки, содержащие ":"
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
