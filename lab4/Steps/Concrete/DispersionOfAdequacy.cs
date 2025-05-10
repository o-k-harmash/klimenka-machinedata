public class DispersionOfAdequacy : IUnitOfCalculation<double>
{
    private double _result;

    public double Calculate(params object[] args)
    {
        var exp_result = (double[])args[0];
        var predict_result = (double[])args[1];

        if (exp_result.Count() != predict_result.Count())
            throw new ArgumentException("Размерности списков должны совпадать");

        int N = Experiment._exp_amount;
        int m = Experiment._exp_repeat;
        int l = 4;
        int fAdequacy = N - l;

        if (fAdequacy <= 0)
            throw new ArgumentException("Степени свободы должны быть положительными");

        double sum = 0;
        for (int i = 0; i < N; i++)
        {
            double diff = exp_result[i] - predict_result[i];
            sum += diff * diff;
        }

        _result = sum * m / fAdequacy;

        return _result;
    }

    public void Report()
    {
        Console.WriteLine($"\n🧮 Дисперсії аддекватности (S^2_адв): {_result}");
    }
}

