using System; // подключаем стандартную библиотеку .NET — используется для Console

namespace Snake // пространство имён проекта, объединяет все игровые классы
{
    // Класс Point — базовая единица, описывающая точку на экране (один символ на координатах X, Y)
    public class Point
    {
        public int x;       // координата X — горизонтальное положение точки
        public int y;       // координата Y — вертикальное положение точки
        public char sym;    // символ, который отображается на экране (например '*', '$', '☻')

        // Пустой конструктор — используется, если нужно создать "пустую" точку
        public Point() { }

        // Конструктор с параметрами — инициализирует точку с координатами и символом
        public Point(int x, int y, char sym)
        {
            this.x = x;         // сохраняем координату X
            this.y = y;         // сохраняем координату Y
            this.sym = sym;     // сохраняем символ, который будет отображён в этой точке
        }

        // Метод рисует символ точки на экране
        public void Draw()
        {
            Console.SetCursorPosition(x, y); // устанавливаем позицию курсора
            Console.Write(sym);              // выводим символ
        }

        // Перегруженный метод Draw — рисует символ с заданным цветом
        public void Draw(ConsoleColor color)
        {
            Console.ForegroundColor = color; // устанавливаем цвет текста
            Console.SetCursorPosition(x, y); // позиционируем курсор
            Console.Write(sym);              // выводим символ
            Console.ResetColor();            // сбрасываем цвет к стандартному
        }

        // Метод очищает точку (заменяет символ на пробел и перерисовывает)
        public void Clear()
        {
            sym = ' '; // заменяем символ на пробел
            Draw();    // перерисовываем точку (она исчезает с экрана)
        }

        // Метод для перемещения точки на определённое количество шагов в указанном направлении
        public void Move(int offset, Direction direction)
        {
            switch (direction) // проверяем направление
            {
                case Direction.RIGHT: x += offset; break;  // вправо по X
                case Direction.LEFT: x -= offset; break;  // влево по X
                case Direction.UP: y -= offset; break;  // вверх по Y
                case Direction.DOWN: y += offset; break;  // вниз по Y
            }
        }

        // Метод проверяет, совпадает ли эта точка с другой по координатам
        public bool IsHit(Point p)
        {
            return p.x == x && p.y == y; // возвращает true, если координаты одинаковые
        }
    }
}
