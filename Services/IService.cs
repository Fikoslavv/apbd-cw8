namespace apbd_cw8;

public interface IService<T> : IEnumerable<T>
{
    public abstract IEnumerable<T> GetData();
    public abstract bool InsertData(T value);
    public abstract bool UpdateData(T value);
    public abstract bool DeleteData(T value);
}
