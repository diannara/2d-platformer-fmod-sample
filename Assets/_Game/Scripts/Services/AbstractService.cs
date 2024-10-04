namespace TIGD.Services
{
    public abstract class AbstractService : IService
    {
        public virtual void Initialize()
        {
            ServiceLocator.Register(this);
        }

        public virtual void Shutdown()
        {
            ServiceLocator.Unregister(this);
        }
    }
}
