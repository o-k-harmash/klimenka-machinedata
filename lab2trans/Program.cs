// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Vector<int>[] templates =
{
    new Vector<int>(new int[]{ 1, 1, -1, -1, 1, 1}),
    new Vector<int>(new int[]{-1, 1, -1, -1, 1, 1}),
    new Vector<int>(new int[]{1, -1, -1, -1, 1, -1})
};

Vector<int> input = new Vector<int>(new int[] { -1, -1, 1, -1, 1, 1 });

var firstLayerOutput = new double[templates.Length];
int n = input.ln;

Console.WriteLine("\n🔢 Входной вектор:");
Console.WriteLine($"X = [{string.Join(", ", input.elements)}]");

Console.WriteLine("\n📦 Первый слой (выходы yⱼ = 0.5 * (X·Tⱼ) + n / 2):");

for (int t = 0; t < templates.Length; t++)
{
    int dot = 0;
    for (int j = 0; j < n; j++)
        dot += templates[t][j] * input[j];

    double yj = 0.5 * dot + n / 2.0;
    firstLayerOutput[t] = yj;

    Console.WriteLine($"Шаблон {t + 1}: X·T = {dot}, y{t + 1} = 0.5 * {dot} + {n / 2.0} = {yj:F2}");
}

Console.WriteLine("\n📊 Матрица весов второго слоя W (взаимное подавление, ε = 0.25):");
var e = 0.25;
for (int j = 0; j < templates.Length; j++)
{
    Console.Write("[ ");
    for (int k = 0; k < templates.Length; k++)
    {
        double w = (j == k) ? 1.0 : -e;
        Console.Write($"{w,6:F2} ");
    }
    Console.WriteLine("]");
}

Console.WriteLine("\n🔁 Итерации второго слоя (взаимное подавление):");
var neuronValueList = firstLayerOutput.ToArray();
bool changed;
int step = 0;
do
{
    changed = false;
    var prev = neuronValueList.ToArray();
    Console.WriteLine($"\n→ Шаг {++step}:");
    for (int j = 0; j < templates.Length; j++)
    {
        Console.WriteLine($"  y{j + 1} (до подавления) = {prev[j]:F4}");
        double netj = prev[j];
        for (int k = 0; k < templates.Length; k++)
            if (k != j)
                netj -= e * prev[k];
        neuronValueList[j] = netj > 0 ? netj : 0;
        Console.WriteLine($"  y{j + 1} (после подавления) = {neuronValueList[j]:F4}");
    }
    for (int i = 0; i < neuronValueList.Length; i++)
    {
        if (Math.Abs(neuronValueList[i] - prev[i]) > 1e-5)
        {
            changed = true;
            break;
        }
    }
} while (changed);

int bestMatchIndex = Array.IndexOf(neuronValueList, neuronValueList.Max());
Console.WriteLine($"\n✅ Найденный шаблон по наибольшей активности: шаблон {bestMatchIndex + 1}");
