using System.Numerics;

public class Matrix<T>
where T : INumber<T>
{
    public Matrix(int row, int col)
    {
        this.row = row;
        this.col = col;
        elements = new T[row, col];
    }

    public Matrix(Matrix<T> mtx)
    {
        row = mtx.row;
        col = mtx.col;
        this.elements = mtx.elements;
    }

    public Matrix(T[,] mtx)
    {
        row = mtx.GetLength(0);
        col = mtx.GetLength(1);
        elements = mtx;
    }

    public Matrix(T[] v)
    {
        row = 1;
        col = v.Length;
        elements = new T[1, col];
        for (int i = 0; i < col; i++)
            elements[0, i] = v[i];
    }

    public int row { get; init; }
    public int col { get; init; }

    /**
     * = null! означает - кто не заинитил того проблема xd:)
     */
    public T[,] elements { get; init; }

    public T this[int row, int col]
    {
        get => elements[row, col];
        set => elements[row, col] = value;
    }

    public void ZeroDig()
    {
        for (int s = 0; s < row; s++)
            elements[s, s] = T.Zero;
    }

    public void SingleDig()
    {
        for (int s = 0; s < row; s++)
            elements[s, s] = T.One;
    }

    public static Matrix<T> operator *(Matrix<T> mtxl, Matrix<T> mtxr)
    {
        int n = mtxl.row; // строки левой матрицы
        int m = mtxl.col; // столбцы левой == строки правой
        int p = mtxr.col; // столбцы правой матрицы
        Matrix<T> mtx = new Matrix<T>(n, p); // результат: [n x p]
        for (int i = 0; i < n; i++)             // строки результата
            for (int j = 0; j < p; j++)         // столбцы результата
            {
                T sum = T.Zero;
                for (int k = 0; k < m; k++)     // проход по общей размерности
                    sum += mtxl[i, k] * mtxr[k, j]; // строка слева × столбец справа
                mtx[i, j] = sum;
            }
        return mtx;
    }

    public static Matrix<T> operator *(T scalar, Matrix<T> mtx)
    {
        var result = new Matrix<T>(mtx.row, mtx.col);
        for (int r = 0; r < mtx.row; r++)
            for (int c = 0; c < mtx.col; c++)
                result[r, c] = mtx[r, c] * scalar;
        return result;
    }

    public static Matrix<T> operator *(Matrix<T> mtx, T scalar)
    {
        return scalar * mtx;
    }

    public override string ToString()
    {
        string str = "";
        var s = 0;
        while (s < row)
        {
            int c = 0;
            while (c < col)
            {
                str += $"{elements[s, c]} ";
                c++;
            }
            str += "\n";
            s++;
        }
        return str;
    }

    /**
     * только с левой матрицей и правым вектором xd:)
     */
    public static Vector<T> operator *(Matrix<T> mtx, Vector<T> v)
    {
        int n = mtx.row; // кол-во наблюдений (строк)
        int m = mtx.col; // кол-во факторов (столбцов)

        /**
         * X * y -> [m] xd:)
         */
        Vector<T> xy = new Vector<T>(m);
        for (int i = 0; i < n; i++)
        {
            T sum = T.Zero;
            for (int k = 0; k < m; k++)
                sum += mtx[i, k] * v[k];
            xy[i] = sum;
        }

        return xy;
    }

    public static Matrix<T> operator ~(Matrix<T> mtx)
    {
        int n = mtx.row; // кол-во наблюдений (строк)
        int m = mtx.col; // кол-во факторов (столбцов)

        /**
         * XT
         */
        Matrix<T> tmtx = new Matrix<T>(m, n);
        for (int i = 0; i < n; i++)
        {
            for (int k = 0; k < m; k++)
            {
                tmtx[k, i] = mtx[i, k];
            }
        }

        return tmtx;
    }

    public static Matrix<T> operator +(Matrix<T> left, Matrix<T> right)
    {
        int n = left.row; // строки
        int m = left.col; // столбцы

        Matrix<T> tmtx = new Matrix<T>(n, m); // ✔️ Правильный порядок

        for (int i = 0; i < n; i++)
        {
            for (int k = 0; k < m; k++)
            {
                // Приводим к dynamic, чтобы сработал оператор +
                tmtx[i, k] = left[i, k] + right[i, k];
            }
        }

        return tmtx;
    }

    //проверять на определитель матрица может не иметь обратной вернет бесконечности
    public static Matrix<T> operator !(Matrix<T> mtx)
    {
        int n = mtx.row;
        int m = mtx.col;

        var a = new T[n, n];
        var inv = new Matrix<T>(n, n);
        for (int i = 0; i < n; i++)
            inv[i, i] = T.One;

        // копия данных
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                a[i, j] = mtx[i, j];

        // Гаусс-Жордан
        for (int i = 0; i < n; i++)
        {
            // ищем максимальный элемент по модулю в колонке i
            int maxRow = i;
            for (int k = i + 1; k < n; k++)
                if (T.Abs(a[k, i]) > T.Abs(a[maxRow, i]))
                    maxRow = k;

            // swap строк i и maxRow
            for (int j = 0; j < n; j++)
            {
                (a[i, j], a[maxRow, j]) = (a[maxRow, j], a[i, j]);
                (inv[i, j], inv[maxRow, j]) = (inv[maxRow, j], inv[i, j]);
            }

            // нормализация ведущего элемента
            T div = a[i, i];
            if (div == T.Zero)
                throw new InvalidOperationException("Matrix is singular (non-invertible)");

            for (int j = 0; j < n; j++)
            {
                a[i, j] /= div;
                inv[i, j] /= div;
            }

            // зануляем все, кроме главного элемента в колонке
            for (int k = 0; k < n; k++)
            {
                if (k == i) continue;
                T factor = a[k, i];
                for (int j = 0; j < n; j++)
                {
                    a[k, j] -= factor * a[i, j];
                    inv[k, j] -= factor * inv[i, j];
                }
            }
        }

        return inv;
    }
}