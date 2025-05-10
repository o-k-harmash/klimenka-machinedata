public class FCriteria : IUnitOfCalculation<double>
{
    private double _result;

    public double Calculate(params object[] args)
    {
        var s_adv = (double)args[0];
        var s_vsp = (double)args[1];

        _result = s_adv / s_vsp;

        return _result;
    }

    public void Report()
    {
        Console.WriteLine($"\n🧮 F-критерий (S^2_адв/S^2_воспр): {_result}");
    }
}

