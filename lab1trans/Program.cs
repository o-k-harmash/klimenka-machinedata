// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Vector<int>[] templates =
{
    //{1, 0, 1, 0} (tested values)
    //{1, 1, 1, 0}
    // { 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1},
    // {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1}]
    new Vector<int>(new int[]{ 1, 1, -1, -1, 1, 1}),
    new Vector<int>(new int[]{-1, 1, -1, -1, 1, 1}),
    new Vector<int>(new int[]{1, -1, -1, -1, 1, -1})
};

//l0-input
//{1, 0, 1, 0} (tested values)
Vector<int> input = new Vector<int>(new int[] { 1, 1, -1, -1, 1, 1 });

//hamming
// var dotProductList = new int[templates.Length];

// Console.WriteLine("🔢 Входной вектор:");
// Console.WriteLine($"X = [{string.Join(", ", input.elements)}]");

// Console.WriteLine("\n📦 Сравнение с шаблонами (скалярное произведение):");
// for (int t = 0; t < templates.Length; t++)
// {
//     int dot = 0;
//     for (int j = 0; j < input.ln; j++)
//         dot += templates[t][j] * input[j];

//     Console.WriteLine($"Шаблон {t + 1}: [{string.Join(", ", templates[t].elements)}] — X·T = {dot}");
//     dotProductList[t] = dot;
// }

// int bestMatchIndex = Array.IndexOf(dotProductList, dotProductList.Max());
// Console.WriteLine($"\n✅ Найденный шаблон по наибольшему скалярному произведению: шаблон {bestMatchIndex + 1}");

Console.WriteLine("\n🧠 Модель Хопфілда — ітеративне відновлення:");
var matrixTemplates = templates.Select(t => new Matrix<int>(t.elements)).ToList();
var weightMatrix = new Matrix<int>(input.ln, input.ln);
var inputMatrix = new Matrix<int>(input.elements);

for (int i = 0; i < templates.Length; i++)
    weightMatrix += ~matrixTemplates[i] * matrixTemplates[i];

weightMatrix.ZeroDig();

Console.WriteLine("\n📈 Матриця ваг (W):");
Console.WriteLine(weightMatrix);

var sign = (int val) =>
    val > 0 ? 1 :
    val < 0 ? -1 : 0;

var current = inputMatrix;
for (int step = 1; step <= 3; step++)
{
    current = current * weightMatrix;

    for (int c = 0; c < current.col; c++)
        current[0, c] = sign(current[0, c]);

    Console.WriteLine($"\n🔁 Крок {step} — X{step} = sign(X * W):");
    Console.WriteLine($"[{string.Join(", ", Enumerable.Range(0, current.col).Select(c => current[0, c]))}]");
}

Console.WriteLine("\n🏁 Фінальний результат:");
Console.WriteLine($"[{string.Join(", ", Enumerable.Range(0, current.col).Select(c => current[0, c]))}]");
