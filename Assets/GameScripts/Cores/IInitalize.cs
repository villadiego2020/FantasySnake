namespace FS.Cores
{
    public interface IInitalize 
    {
        void Initialize(params object[] objects);
        void Deinitialize();
        void Register();
        void Unregister();
    }
}