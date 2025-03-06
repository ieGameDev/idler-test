using Game.Scripts.Data;
using Game.Scripts.Infrastructure.Services.Factory;
using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Progress
{
    public class SaveLoadService : ISaveLoadService
    {
        private readonly IProgressService _progressService;
        private readonly IGameFactory _factory;

        public SaveLoadService(IProgressService progressService, IGameFactory factory)
        {
            _progressService = progressService;
            _factory = factory;
        }

        public void SaveProgress()
        {
            foreach (ISavedProgress progressWriter in _factory.ProgressWriters)
                progressWriter.SaveProgress(_progressService.Progress);
            
            PlayerPrefs.SetString(Constants.Progress.ProgressKey, _progressService.Progress.ToJson());
            
            Debug.Log("[SaveLoadService] Saved");
        }

        public PlayerProgress LoadProgress() =>
            PlayerPrefs.GetString(Constants.Progress.ProgressKey)?.ToDeserialized<PlayerProgress>();
    }
}