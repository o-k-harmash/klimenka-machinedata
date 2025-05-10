
public class Predictor : IUnitOfCalculation<double[]>
{
    private double[] _result = null!;

    private double predict(double[] coefficients, double[] x)
    {
        if (coefficients.Length != 4 || x.Length != 3)
            throw new ArgumentException("Ожидается 4 коэффициента и 3 входных признака");

        double yPred = coefficients[0]; // β0 — свободный член
        for (int i = 0; i < 3; i++)
            yPred += coefficients[i + 1] * x[i]; // β1*x1 + β2*x2 + β3*x3

        return yPred;
    }

    public double[] Calculate(params object[] args)
    {
        var coef = (double[])args[0];
        var repeat_len = Experiment.normalizeFactorValuesMatrix.GetLength(0);

        _result = new double[repeat_len];

        for (int i = 0; i < repeat_len; i++)
        {
            // Денормализация факторов
            double x1 = Experiment.normalizeFactorValuesMatrix[i, 0] == -1 ? Experiment.x1min : Experiment.x1max;
            double x2 = Experiment.normalizeFactorValuesMatrix[i, 1] == -1 ? Experiment.x2min : Experiment.x2max;
            double x3 = Experiment.normalizeFactorValuesMatrix[i, 2] == -1 ? Experiment.x3min : Experiment.x3max;

            // Предсказание по денормализованным коэффициентам
            double yPred = predict(coef, new[] { x1, x2, x3 });
            _result[i] = yPred;
        }

        return _result;
    }

    public void Report()
    {
        Console.WriteLine($"\n📉 Предсказанные результаты на основе регрессии");
        for (int i = 0; i < _result.Length; i++)
        {
            Console.WriteLine($"predicted{i} = {_result[i]:F4}");
        }
    }
}

