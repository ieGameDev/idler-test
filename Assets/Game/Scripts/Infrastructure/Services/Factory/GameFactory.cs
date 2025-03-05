using Game.Scripts.AssetManager;
using Game.Scripts.Infrastructure.Services.Input;
using Game.Scripts.Player;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IInputService _inputService;

        private GameObject _player;

        public GameFactory(IInputService inputService)
        {
            _inputService = inputService;
        }

        public GameObject CreatePlayer(Transform spawnPoint)
        {
            _player = Object.Instantiate(Resources.Load<GameObject>(AssetAddress.Player.PlayerPath), spawnPoint);
            
            PlayerMovement playerMovement = _player.GetComponent<PlayerMovement>();
            playerMovement.Construct(_inputService);
            
            return _player;
        }

        public GameObject CreatePlayerUI()
        {
            GameObject playerUI = Object.Instantiate(Resources.Load<GameObject>(AssetAddress.Player.HUDPath));
            return playerUI;
        }
    }
}