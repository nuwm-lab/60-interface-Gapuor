using System;

namespace SystemsExample
{
    // 1. Інтерфейс для системи лінійних алгебраїчних рівнянь (СЛАР)
    // Визначає контракт для всіх класів, що працюють з СЛАР.
    public interface ISLAESystem
    {
        void SetCoefficients();
        void Display();
        void CheckVector(double[] X);
        double[] ReadVector();
        int Size { get; }
    }

    // 2. Базовий АБСТРАКТНИЙ клас для систем рівнянь NxN
    // Реалізує інтерфейс ISLAESystem та загальну логіку для будь-якого розміру N.
    abstract class SystemNxN : ISLAESystem
    {
        protected int _size;
        protected double[,] _a;
        protected double[] _b;
        protected const double EPSILON = 1e-6;

        public int Size => _size; // Реалізація властивості з інтерфейсу

        public SystemNxN(int n)
        {
            _size = n;
            _a = new double[n, n];
            _b = new double[n];
        }

        // Деструктор (фіналізатор)
        ~SystemNxN()
        {
            // Вивід повідомлення про знищення об'єкта
            Console.WriteLine($"Об'єкт SystemNxN розміром {_size}×{_size} знищено.");
        }

        // Безпечне зчитування числа
        protected double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (input == null)
                {
                    Console.WriteLine("Ввід перервано. Спробуйте ще раз.");
                    continue;
                }

                // Використання System.Globalization.CultureInfo.InvariantCulture
                // для коректного парсингу чисел з крапкою/комою незалежно від регіональних налаштувань
                if (double.TryParse(input.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double result))
                    return result;

                Console.WriteLine("Некоректне значення. Введіть число ще раз.");
            }
        }

        // Введення коефіцієнтів системи (може бути перевизначено)
        public virtual void SetCoefficients()
        {
            Console.WriteLine($"\nВведіть коефіцієнти для системи {_size}×{_size}:");
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                    _a[i, j] = ReadDouble($"a[{i + 1},{j + 1}] = ");
                _b[i] = ReadDouble($"b[{i + 1}] = ");
            }
        }

        // Виведення системи (може бути перевизначено)
        public virtual void Display()
        {
            Console.WriteLine($"\nСистема рівнянь {_size}×{_size}:");
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                    Console.Write($"{_a[i, j]}*x{j + 1}" + (j < _size - 1 ? " + " : ""));
                Console.WriteLine($" = {_b[i]}");
            }
        }

        // Перевірка вектора (може бути перевизначено)
        public virtual void CheckVector(double[] X)
        {
            if (X.Length != _size)
            {
                Console.WriteLine($"Довжина вектора {X.Length} не відповідає розміру системи {_size}.");
                return;
            }

            Console.WriteLine($"\nПеревірка вектора розміром {_size}:");
            bool satisfies = true;

            for (int i = 0; i < _size; i++)
            {
                double left = 0;
                for (int j = 0; j < _size; j++)
                    left += _a[i, j] * X[j];

                Console.WriteLine($"Рівняння {i + 1}: Ліва частина: {left:F6}, Права частина: {_b[i]:F6}");
                if (Math.Abs(left - _b[i]) > EPSILON)
                {
                    satisfies = false;
                    Console.WriteLine($"---> Невідповідність (різниця: {Math.Abs(left - _b[i]):E2})");
                }
            }

            Console.WriteLine(satisfies
                ? "Вектор задовольняє систему (в межах точності)."
                : "Вектор не задовольняє систему.");
        }

        // Введення вектора (не перевизначається, оскільки логіка однакова для всіх розмірів)
        public double[] ReadVector()
        {
            double[] X = new double[_size];
            Console.WriteLine($"\nВведіть компоненти вектора розміром {_size}:");
            for (int i = 0; i < _size; i++)
                X[i] = ReadDouble($"x{i + 1} = ");
            return X;
        }
    }

    // Похідний клас для системи 2×2
    class System2x2 : SystemNxN
    {
        public System2x2() : base(2)
        {
            Console.WriteLine("\nСтворено об'єкт системи 2×2");
        }

        // Деструктор (фіналізатор)
        ~System2x2()
        {
            Console.WriteLine("Об'єкт системи 2×2 знищено.");
        }

        // Перевизначення для виведення специфічного повідомлення
        public override void SetCoefficients()
        {
            Console.WriteLine("\n=== Налаштування системи 2×2 ===");
            base.SetCoefficients();
        }

        // Перевизначення для виведення специфічного повідомлення
        public override void Display()
        {
            Console.WriteLine("\n=== Вивід системи 2×2 ===");
            base.Display();
        }

        // Перевизначення для виведення специфічного повідомлення
        public override void CheckVector(double[] X)
        {
            Console.WriteLine("\n=== Перевірка рішення для системи 2×2 ===");
            base.CheckVector(X);
        }
    }

    // Похідний клас для системи 3×3 (СЛАР 3x3)
    class SLAR3x3 : SystemNxN
    {
        public SLAR3x3() : base(3)
        {
            Console.WriteLine("\nСтворено об'єкт системи 3×3 (СЛАР)");
        }

        // Деструктор (фіналізатор)
        ~SLAR3x3()
        {
            Console.WriteLine("Об'єкт системи 3×3 (СЛАР) знищено.");
        }

        // Перевизначення для виведення специфічного повідомлення
        public override void SetCoefficients()
        {
            Console.WriteLine("\n=== Налаштування СЛАР 3×3 ===");
            base.SetCoefficients();
        }

        // Перевизначення для виведення специфічного повідомлення
        public override void Display()
        {
            Console.WriteLine("\n=== Вивід СЛАР 3×3 ===");
            base.Display();
        }

        // Перевизначення для виведення специфічного повідомлення
        public override void CheckVector(double[] X)
        {
            Console.WriteLine("\n=== Перевірка рішення для СЛАР 3×3 ===");
            base.CheckVector(X);
        }
    }

    // Основний клас програми
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Демонстрація роботи з системами лінійних рівнянь (СЛАР) та використання інтерфейсу.");

            try
            {
                // Створення об'єктів та демонстрація поліморфізму через інтерфейс
                // Хоча змінні оголошені конкретними типами, вони реалізують ISLAESystem.
                // Демонстрація роботи з системою 2x2
                ISLAESystem sys2 = new System2x2();
                sys2.SetCoefficients();
                sys2.Display();
                var X2 = sys2.ReadVector();
                sys2.CheckVector(X2);
                
                // Демонстрація роботи з системою 3x3
                ISLAESystem sys3 = new SLAR3x3();
                sys3.SetCoefficients();
                sys3.Display();
                var X3 = sys3.ReadVector();
                sys3.CheckVector(X3);

                // Приклад використання інтерфейсу: перевірка вектора з неправильним розміром
                Console.WriteLine("\n--- Перевірка функціоналу інтерфейсу з іншим об'єктом ---");
                double[] wrongSizeVector = new double[] { 1.0, 2.0, 3.0, 4.0 };
                Console.WriteLine($"Спроба перевірити 4-мірний вектор для системи 2х2:");
                sys2.CheckVector(wrongSizeVector);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nКритична помилка виконання: {ex.Message}");
            }

            // Додаткова затримка, щоб побачити виведення деструкторів, коли збирач сміття спрацює
            Console.WriteLine("\nПрограму завершено. Знищення об'єктів буде відбуватися після виходу з try блоку, коли збирач сміття спрацює.");
            Console.WriteLine("Натисніть будь-яку клавішу...");
            Console.ReadKey();
        }
    }
}
