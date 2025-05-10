public class ReproducibilityDispersion : IUnitOfCalculation<double>
{
    private double _result;

    public double Calculate(params object[] args)
    {
        var rows_dispersion = (double[])args[0];

        _result = rows_dispersion.Average();

        return _result;
    }

    public void Report()
    {
        Console.WriteLine($"🧾 Дисперсія відтворюваності: S²_y = {_result:F4}");
    }
}

