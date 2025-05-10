public interface IUnitOfCalculation<T>
{
    public T Calculate(params object[] args);
    public void Report();
}