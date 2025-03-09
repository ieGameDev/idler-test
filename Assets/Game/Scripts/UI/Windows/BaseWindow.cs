using Game.Scripts.Data;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Infrastructure.Services.StaticData;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Windows
{
    public abstract class BaseWindow : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;

        protected IProgressService ProgressService;
        protected IStaticDataService StaticDataService;

        protected PlayerProgress Progress => ProgressService.Progress;

        public void Construct(IProgressService progressService, IStaticDataService staticDataService)
        {
            ProgressService = progressService;
            StaticDataService = staticDataService;
        }

        private void Awake() => OnAwake();

        private void Start()
        {
            Initialize();
            SubscribeUpdates();
        }

        private void OnDestroy() => CleanUp();

        protected virtual void OnAwake() =>
            _closeButton.onClick.AddListener(() => Destroy(gameObject));

        protected virtual void Initialize()
        {
        }

        protected virtual void SubscribeUpdates()
        {
        }

        protected virtual void CleanUp()
        {
        }
    }
}