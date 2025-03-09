using Game.Scripts.Infrastructure.DI;
using Game.Scripts.UI.Windows;

namespace Game.Scripts.Infrastructure.Services.UI
{
    public interface IWindowService : IService
    {
        void Open(WindowId windowId);
    }
}