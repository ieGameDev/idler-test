using Game.Scripts.Infrastructure.Services.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class OpenWindowButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private WindowId _windowId;
        
        private IWindowService _windowService;

        public void Construct(IWindowService windowService) => 
            _windowService = windowService;

        private void Awake() => 
            _button.onClick.AddListener(OpenWindow);

        private void OpenWindow() => 
            _windowService.Open(_windowId);
    }
}