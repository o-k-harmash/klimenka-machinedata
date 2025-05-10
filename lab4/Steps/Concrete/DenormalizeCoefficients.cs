public class DenormalizeCoefficients : IUnitOfCalculation<double[]>
{
    private double[] _result = null!;

    public double[] Calculate(params object[] args)
    {
        var coef_manual = (double[])args[0];

        var dx1 = (Experiment.x1max - Experiment.x1min) / 2.0;
        var dx2 = (Experiment.x2max - Experiment.x2min) / 2.0;
        var dx3 = (Experiment.x3max - Experiment.x3min) / 2.0;

        var x10 = (Experiment.x1max + Experiment.x1min) / 2.0;
        var x20 = (Experiment.x2max + Experiment.x2min) / 2.0;
        var x30 = (Experiment.x3max + Experiment.x3min) / 2.0;

        var a1 = coef_manual[1] / dx1;
        var a2 = coef_manual[2] / dx2;
        var a3 = coef_manual[3] / dx3;

        var a0 = coef_manual[0] - a1 * x10 - a2 * x20 - a3 * x30;

        _result = new double[] { a0, a1, a2, a3 };

        return _result;
    }

    public void Report()
    {
        Console.WriteLine("\n🔄 Коефіцієнти у фізичних одиницях (денормалізовані):");
        for (int i = 0; i < _result.Length; i++)
        {
            Console.WriteLine($"a{i} = {_result[i]:F4}");
        }
    }
}

