public class Dispersion : IUnitOfCalculation<double[]>
{
    private double[] _result = null!;

    public double[] Calculate(params object[] args)
    {
        var exp_result = (double[,])args[0];

        int combinationsCount = exp_result.GetLength(0);
        int expCount = exp_result.GetLength(1);

        _result = new double[combinationsCount];

        for (int i = 0; i < combinationsCount; i++)
        {
            double sum = 0;
            double sumSq = 0;

            for (int j = 0; j < expCount; j++)
            {
                double y = exp_result[i, j];
                sum += y;
                sumSq += y * y;
            }

            double mean = sum / expCount;
            double variance = (sumSq - expCount * mean * mean) / (expCount - 1); // S^2 = (∑y² - n·mean²) / (n-1)

            _result[i] = variance;
        }

        return _result;
    }

    public void Report()
    {
        Console.WriteLine("\n🧮 Дисперсії по рядках (S^2_ij):");
        for (int i = 0; i < _result.Length; i++)
        {
            Console.WriteLine($"S²_{i + 1} = {_result[i]:F4}");
        }
    }
}

