public class Experiment : IUnitOfCalculation<double[,]>
{
    // === 🔧 Исходные параметры пользователя (их можно изменить для своего варианта)
    private static readonly int a = 2;
    private static readonly int b = a;
    private static readonly int c = a;

    // === 📐 Коэффициенты модели (рассчитываются на основе a, b, c)
    private static readonly double a0 = 1 + c;
    private static readonly double a1 = 0.5 + (b - c) / 5.0;
    private static readonly double a2 = 0.1 + (a + b + c) / 5.0;
    private static readonly double a3 = 0.3 + (1 + c) * (1 + b) / 10.0;
    private static readonly double a4 = 0.3 + (a - b + c) / 5.0;

    // === 📊 Дисперсия (используется в генерации случайного шума в y)
    private static readonly double dispersion = (2.0 + a) / (5 + b + c);

    // === 📏 Диапазоны факторов (min / max), рассчитываются из a, b, c
    public static readonly double x1min = (a + b + 2 * c) / 2;
    public static readonly double x2min = (a + 1) * (b + 2) / (c + 1);
    public static readonly double x3min = a + 3 * b + c;

    public static readonly double x1max = 1.3 * x1min;
    public static readonly double x2max = 1.4 * x2min;
    public static readonly double x3max = 1.5 * x3min;

    // === 📋 Физическая матрица факторов (x1, x2, x3)
    public static readonly double[,] factorValuesMatrix =
    {
        {  x1min, x2min, x3min },
        {  x1min, x2min, x3max },
        {  x1min, x2max, x3min },
        {  x1min, x2max, x3max },
        {  x1max, x2min, x3min },
        {  x1max, x2min, x3max },
        {  x1max, x2max, x3min },
        {  x1max, x2max, x3max },
    };

    // === 🧪 Нормализованная матрица [-1; 1] для факторного анализа
    public static readonly int[,] normalizeFactorValuesMatrix =
    {
        { -1, -1, -1 },
        { -1, -1,  1 },
        { -1,  1, -1 },
        { -1,  1,  1 },
        {  1, -1, -1 },
        {  1, -1,  1 },
        {  1,  1, -1 },
        {  1,  1,  1 },
    };

    public static readonly double[,] extensebleNormalizeFactorValuesMatrix =
    {
        { 1, -1, -1, -1 },
        { 1, -1, -1,  1 },
        { 1, -1,  1, -1 },
        { 1, -1,  1,  1 },
        { 1,  1, -1, -1 },
        { 1,  1, -1,  1 },
        { 1,  1,  1, -1 },
        { 1,  1,  1,  1 },
    };

    // === ⚙️ Модель системы (расчет ŷ по факторам)
    private static double makeExperiment(double x1, double x2, double x3)
    {
        return
            -1 * x3 * x1 * x2 * a3 * a4 +
            0.76 * a1 * x1 * a2 * x2 +
            1.83 * x3 * x1 * a3 * a4 -
            x3 * x2 * a3 +
            0.76 * a1 * x1 +
            1.83 * x3 * a3 +
            a0 +
            generateRandomValue();
    }

    // === 🎲 Генерация случайного отклонения для добавления к y (нормальное распределение)
    private static double generateRandomValue()
    {
        var random = new Random();
        double mean = 0;
        double stdDev = Math.Sqrt(dispersion);

        // Используем метод Бокса-Мюллера для нормального распределения
        double u1 = 1.0 - random.NextDouble(); // [0,1)
        double u2 = 1.0 - random.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

        return mean + stdDev * randStdNormal;
    }

    // === 🔁 Параметры эксперимента
    public static int _exp_amount = 8; // Количество комбинаций (2^3)
    public static int _exp_repeat = 5; // Повторы на каждую точку
    private double[,] _result = null!;  // Результирующая матрица

    // === 📌 Основной расчет (запускает имитацию эксперимента по всем точкам)
    public double[,] Calculate(params object[] args)
    {
        int combinationsCount = factorValuesMatrix.GetLength(0); // = 8
        _result = new double[combinationsCount, _exp_repeat]; // 8 строк, 5 столбцов

        for (int i = 0; i < combinationsCount; i++)
        {
            var x1 = factorValuesMatrix[i, 0];
            var x2 = factorValuesMatrix[i, 1];
            var x3 = factorValuesMatrix[i, 2];

            for (int j = 0; j < _exp_repeat; j++)
            {
                _result[i, j] = makeExperiment(x1, x2, x3); // <-- расчёт y
            }
        }

        return _result;
    }

    // === 🖨️ Вывод результатов эксперимента (матрица Y)
    public void Report()
    {
        Console.WriteLine("🧪 Повний факторний експеримент (ПФЕ)");
        Console.WriteLine($"Кількість повторень на точку: {_exp_repeat}\n");

        Console.WriteLine("🔍 Вхідні дані експерименту:");
        Console.WriteLine($"a = {a}, b = {b}, c = {c}");
        Console.WriteLine($"x1min = {x1min}, x1max = {x1max}");
        Console.WriteLine($"x2min = {x2min}, x2max = {x2max}");
        Console.WriteLine($"x3min = {x3min}, x3max = {x3max}\n");
        Console.WriteLine($"Дисперсія: {dispersion:F4}");
        Console.WriteLine($"Коефіцієнти моделі: a0 = {a0:F4}, a1 = {a1:F4}, a2 = {a2:F4}, a3 = {a3:F4}, a4 = {a4:F4}\n");

        // 1. Вывод значений Y по всем комбинациям
        Console.WriteLine("📊 Результати експерименту (Y):");
        for (int i = 0; i < _exp_amount; i++)
        {
            Console.Write($"Комбінація {i + 1}: ");
            for (int j = 0; j < _exp_repeat; j++)
            {
                Console.Write($"{_result[i, j]:F4} ");
            }
            Console.WriteLine();
        }
    }
}

