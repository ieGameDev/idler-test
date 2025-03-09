using Game.Scripts.Infrastructure.Services.Factory;

namespace Game.Scripts.Infrastructure.Services.UI
{
    public class WindowService : IWindowService
    {
        private readonly IUIFactory _uiFactory;

        public WindowService(IUIFactory uiFactory) => _uiFactory = uiFactory;

        public void Open(WindowId windowId)
        {
            switch (windowId)
            {
                case WindowId.Unknown:
                    break;
                case WindowId.Upgrades:
                    _uiFactory.CreateUpgradesWindow();
                    break;
            }
        }
    }
}