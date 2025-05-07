using System;                    // используется для Random, Console и пр.
using System.Collections.Generic; // для списка wallList

namespace Snake // пространство имён, объединяющее все игровые классы
{
    // Класс Walls — управляет всеми стенами на карте (рамка + дополнительные препятствия)
    public class Walls
    {
        private List<Figure> wallList; // список всех стен в игре (каждая стена — это объект Figure)

        private int mapWidth;          // ширина карты
        private int mapHeight;         // высота карты
        private Random rand = new Random(); // генератор случайных чисел (для генерации стен)

        // Конструктор: задаёт размеры карты и создаёт начальную рамку по периметру
        public Walls(int width, int height)
        {
            mapWidth = width;
            mapHeight = height;
            wallList = new List<Figure>(); // создаём список, куда будут добавляться все стены

            // Добавляем четыре края карты: верх, низ, левая и правая граница
            wallList.Add(new HorizontalLine(0, width - 1, 0, '+'));               // верхняя граница
            wallList.Add(new HorizontalLine(0, width - 1, height - 1, '+'));      // нижняя граница
            wallList.Add(new VerticalLine(0, height - 1, 0, '+'));                // левая граница
            wallList.Add(new VerticalLine(0, height - 1, width - 1, '+'));        // правая граница
        }

        // Метод отрисовки всех стен из списка wallList
        public void Draw()
        {
            foreach (var wall in wallList) // перебираем все стены
            {
                wall.Draw();               // вызываем метод Draw() из класса Figure (наследуемого)
            }
        }

        // Проверка: врезалась ли змейка в какую-либо стену
        public bool IsHit(Snake snake)
        {
            Point next = snake.GetNextPoint(); // узнаём, куда движется голова змейки

            foreach (var wall in wallList)     // проверяем каждую стену
            {
                if (wall.IsHit(next))          // если столкновение найдено
                    return true;               // возвращаем true — игра окончена
            }

            return false; // если ни в одну стену не врезалась
        }

        // Метод для генерации случайной стены внутри карты (на новом уровне)
        public void AddRandomWall()
        {
            int length = rand.Next(4, 10);             // случайная длина стены от 4 до 9 точек
            int x = rand.Next(5, mapWidth - 10);       // координата X (с отступом от края)
            int y = rand.Next(3, mapHeight - 3);       // координата Y (тоже не вплотную к краю)

            Figure newWall;

            // Случайно выбираем горизонтальную или вертикальную стену
            if (rand.Next(2) == 0)
                newWall = new HorizontalLine(x, x + length, y, '#'); // горизонтальная
            else
                newWall = new VerticalLine(y, y + length, x, '#');   // вертикальная

            wallList.Add(newWall); // добавляем новую стену в список
            newWall.Draw();        // сразу отрисовываем её на экране
        }
    }
}
