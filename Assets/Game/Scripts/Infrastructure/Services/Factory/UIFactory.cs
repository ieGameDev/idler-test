using Game.Scripts.AssetManager;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Infrastructure.Services.UI;
using Game.Scripts.Logic.ScriptableObjects;
using Game.Scripts.UI.Windows;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IStaticDataService _staticData;
        private readonly IProgressService _progressService;

        private Transform _uiRoot;

        public UIFactory(IStaticDataService staticData, IProgressService progressService)
        {
            _staticData = staticData;
            _progressService = progressService;
        }

        public void CreateUpgradesWindow()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Upgrades);
            BaseWindow window = Object.Instantiate(config.Window, _uiRoot);
            window.Construct(_progressService, _staticData);
        }

        public void CreateUIRoot() =>
            _uiRoot = Object.Instantiate(Resources.Load<GameObject>(AssetAddress.Player.UIRootPath)).transform;
    }
}