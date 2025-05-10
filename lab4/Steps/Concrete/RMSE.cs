
public class RMSE : IUnitOfCalculation<double>
{
    private double _result;

    public double Calculate(params object[] args)
    {
        var predicted = (double[])args[0];
        var actual = (double[])args[1];

        if (predicted.Length != actual.Length)
            throw new ArgumentException("Длины массивов должны совпадать");

        double sumSq = 0;
        for (int i = 0; i < predicted.Length; i++)
        {
            double error = predicted[i] - actual[i];
            sumSq += error * error;
        }

        _result = Math.Sqrt(sumSq / predicted.Length);

        return _result;
    }

    public void Report()
    {
        Console.WriteLine($"\n📉 Середньоквадратичне відхилення між y та ŷ: RMSE = {_result:F4}");
    }
}

