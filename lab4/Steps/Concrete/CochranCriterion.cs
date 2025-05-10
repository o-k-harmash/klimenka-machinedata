public class CochranCriterion : IUnitOfCalculation<double>
{
    private double _result;

    public double Calculate(params object[] args)
    {
        var rows_dispersion = (double[])args[0];

        double max = rows_dispersion.Max();
        double sum = rows_dispersion.Sum();
        _result = max / sum;

        return _result;
    }

    public void Report()
    {
        Console.WriteLine($"\n📏 Критерій Кохрена: G = {_result:F4}");
    }
}

