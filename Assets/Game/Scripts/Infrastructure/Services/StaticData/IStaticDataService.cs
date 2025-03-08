using Game.Scripts.Infrastructure.DI;
using Game.Scripts.Logic.ScriptableObjects;

namespace Game.Scripts.Infrastructure.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void LoadCustomer();
        CustomerData DataForCustomer();
    }
}