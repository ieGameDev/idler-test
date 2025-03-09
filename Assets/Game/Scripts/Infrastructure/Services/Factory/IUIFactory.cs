using Game.Scripts.Infrastructure.DI;

namespace Game.Scripts.Infrastructure.Services.Factory
{
    public interface IUIFactory : IService
    {
        void CreateUpgradesWindow();
        void CreateUIRoot();
    }
}