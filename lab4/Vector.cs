using System.Numerics;

public class Vector<T>
where T : INumber<T>
{
    public int ln { get; init; }

    public Vector(T[] v)
    {
        ln = v.Length;
        elements = v;
    }

    public Vector(int ln)
    {
        this.ln = ln;
        elements = new T[ln];
    }

    /**
     * = null! означает - кто не заинитил того проблема xd:)
     */
    public T[] elements { get; set; } = null!;

    public T this[int inx]
    {
        get => elements[inx];
        set => elements[inx] = value;
    }
}