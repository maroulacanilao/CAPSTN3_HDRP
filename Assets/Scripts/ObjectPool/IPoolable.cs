namespace ObjectPool
{
    public interface IPoolable
    {
        void OnSpawn();
        void OnDeSpawn();
    }
}