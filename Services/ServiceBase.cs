using System.Collections;

namespace apbd_cw8;

public abstract class ServiceBase<T> : IService<T>
{
    protected readonly string connectionString;

    public ServiceBase(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public abstract IEnumerable<T> GetData();

    public abstract bool InsertData(T value);
    public abstract bool UpdateData(T value);
    public abstract bool DeleteData(T value);

    public IEnumerator<T> GetEnumerator() => this.GetData().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
