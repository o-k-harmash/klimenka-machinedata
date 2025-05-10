
public class StudentCriteria : IUnitOfCalculation<double[]>
{
    private readonly double tCritical = 2.306;
    private double[] _result = null!;

    private static double betaCoefficientStdError(double[] xk, double sigma)
    {
        double sumSquares = 0;
        foreach (var x in xk)
            sumSquares += x * x;

        if (sumSquares == 0)
            throw new DivideByZeroException("Сумма квадратов по признаку равна 0");

        return sigma / Math.Sqrt(sumSquares);
    }

    public double[] Calculate(params object[] args)
    {
        var rmse = (double)args[0];
        var coefficients = (double[])args[1];

        int n = Experiment.extensebleNormalizeFactorValuesMatrix.GetLength(0); // число наблюдений
        int m = Experiment.extensebleNormalizeFactorValuesMatrix.GetLength(1); // число признаков

        double[] tValues = new double[m];

        for (int i = 0; i < m; i++)
        {
            double[] xk = new double[n];
            for (int j = 0; j < n; j++)
                xk[j] = Experiment.extensebleNormalizeFactorValuesMatrix[j, i]; // строка j, признак i

            double stdErr = betaCoefficientStdError(xk, rmse);
            tValues[i] = Math.Abs(coefficients[i]) / stdErr;
        }
        _result = tValues;

        return _result;
    }

    public void Report()
    {
        for (int i = 0; i < _result.Length; i++)
        {
            bool significant = _result[i] > tCritical;
            Console.WriteLine($"t_{i} = {_result[i]:F4} → {(significant ? "значимий" : "НЕзначимий")}");
        }
    }
}

