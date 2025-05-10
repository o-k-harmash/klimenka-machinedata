
public class AverageExperiment : IUnitOfCalculation<double[]>
{
    private double[] _result = null!;

    public double[] Calculate(params object[] args)
    {
        var value_matrix = (double[,])args[0];

        int combinationsCount = value_matrix.GetLength(0);
        int expCount = value_matrix.GetLength(1);

        _result = new double[combinationsCount];

        for (int i = 0; i < combinationsCount; i++)
        {
            double sum = 0;
            for (int j = 0; j < expCount; j++)
            {
                sum += value_matrix[i, j];
            }
            _result[i] = sum / expCount;
        }

        return _result;
    }

    public void Report()
    {
        Console.WriteLine($"\n📈 Середні значення відгуку (Ȳ):");
        for (int i = 0; i < _result.Length; i++)
        {
            Console.WriteLine($"Ȳ_{i + 1} = {_result[i]:F4}");
        }
    }
}

