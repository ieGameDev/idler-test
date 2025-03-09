using Game.Scripts.Data;
using Game.Scripts.Infrastructure.DI;
using Game.Scripts.Infrastructure.Services.Progress;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class CashCounter : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private TextMeshProUGUI _counter;

        private WorldData _worldData;
        private ISaveLoadService _saveLoadService;

        public void Construct(WorldData worldData)
        {
            _worldData = worldData;
            _worldData.CashData.OnChanged += UpdateCounter;
        }
        
        private void Awake() => 
            _saveLoadService = DiContainer.Instance.Single<ISaveLoadService>();

        private void OnDestroy() =>
            _worldData.CashData.OnChanged -= UpdateCounter;

        public void SaveProgress(PlayerProgress progress) => 
            progress.WorldData.CashData.Collected = _worldData.CashData.Collected;

        public void LoadProgress(PlayerProgress progress)
        {
            _worldData.CashData.Collected = progress.WorldData.CashData.Collected;
            UpdateCounter();
        }

        private void UpdateCounter()
        {
            _counter.text = $"${_worldData.CashData.Collected}";
            _saveLoadService.SaveProgress();
        }
    }
}