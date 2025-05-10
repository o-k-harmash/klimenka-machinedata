
public class RegressionCoefficientsManual : IUnitOfCalculation<double[]>
{
    private double[] _result = null!;

    public double[] Calculate(params object[] args)
    {
        //вектора и матрицы методы только в 1 месте используются возможно реализацию перенести сюда
        var exp_average_results = new Vector<double>((double[])args[0]);
        var x_matrix = new Matrix<double>(Experiment.extensebleNormalizeFactorValuesMatrix);

        var xt = ~x_matrix;
        var xtx = xt * x_matrix;
        //XT*X → [m x m]


        // Вектор XT * y → [m]
        var xty = xt * exp_average_results;

        //(XT*X)^-1 → [m x m]
        var xtxInv = !xtx;

        _result = (xtxInv * xty).elements;
        return _result;
    }

    public void Report()
    {
        Console.WriteLine("\n📐 Коефіцієнти нормалізованого рівняння регресії:");
        for (int i = 0; i < _result.Length; i++)
        {
            Console.WriteLine($"b{i} = {_result[i]:F4}");
        }
    }
}

