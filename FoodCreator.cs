using System; // подключаем стандартную библиотеку .NET — используем Random

namespace Snake // пространство имён проекта, объединяет все классы игры
{
    // Класс отвечает за создание еды в случайной точке на карте
    public class FoodCreator
    {
        private int mapWidth;  // ширина игрового поля
        private int mapHeight; // высота игрового поля
        private char sym;      // символ, который будет отображаться как еда (например '$')
        private Random rand = new Random(); // генератор случайных чисел для выбора позиции еды

        // Конструктор — принимает ширину, высоту и символ еды
        public FoodCreator(int mapWidth, int mapHeight, char sym)
        {
            this.mapWidth = mapWidth;   // сохраняем ширину карты
            this.mapHeight = mapHeight; // сохраняем высоту карты
            this.sym = sym;             // сохраняем символ еды (например '$')
        }

        // Метод создаёт точку с едой в случайной позиции
        public Point CreateFood()
        {
            // Координаты X и Y выбираются случайно, но не у краёв карты (поэтому от 2 до ширина - 2)
            int x = rand.Next(2, mapWidth - 2);
            int y = rand.Next(2, mapHeight - 2);

            // Возвращаем новую точку еды с заданным символом
            return new Point(x, y, sym);
        }
    }
}
