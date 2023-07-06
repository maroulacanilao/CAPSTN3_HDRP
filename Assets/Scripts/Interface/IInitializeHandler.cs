public interface IInitializeHandler
{
    public void Initialize();
}

public interface IInitializeHandler<in T>
{
    public void Initialize(T data_);
}

public interface IInitializeHandler<in T1, in T2>
{
    public void Initialize(T1 data1_, T2 data2_);
}

public interface IDeInitializeHandler
{
    public void DeInitialize();
}
