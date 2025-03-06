using System.Collections.Generic;
using Game.Scripts.AssetManager;
using Game.Scripts.Infrastructure.Services.Input;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Player;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IInputService _inputService;

        private GameObject _player;

        public List<ISavedProgressReader> ProgressReaders { get; } = new();
        public List<ISavedProgress> ProgressWriters { get; } = new();

        public GameFactory(IInputService inputService)
        {
            _inputService = inputService;
        }

        public GameObject CreatePlayer(Transform spawnPoint)
        {
            _player = Object.Instantiate(Resources.Load<GameObject>(AssetAddress.Player.PlayerPath), spawnPoint);
            
            PlayerMovement playerMovement = _player.GetComponent<PlayerMovement>();
            playerMovement.Construct(_inputService);
            
            RegisterProgressWatchers(_player);
            
            return _player;
        }

        public GameObject CreatePlayerUI()
        {
            GameObject playerUI = Object.Instantiate(Resources.Load<GameObject>(AssetAddress.Player.HUDPath));
            return playerUI;
        }
        
        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }
        
        private void RegisterProgressWatchers(GameObject obj)
        {
            foreach (ISavedProgressReader progressReader in obj.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);

            ProgressReaders.Add(progressReader);
        }
    }
}