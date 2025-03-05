using Game.Scripts.Infrastructure.Services.Factory;
using Game.Scripts.Logic;
using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Infrastructure.GameStates
{
    public class LoadLevelState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;

        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, IGameFactory gameFactory)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
        }

        public void Enter()
        {
            _sceneLoader.LoadScene(Constants.Levels.Main, OnLoaded);
        }

        public void Exit()
        {
        }

        private void OnLoaded()
        {
            InitGameWorld();

            _stateMachine.Enter<GameLoopState>();
        }

        private void InitGameWorld()
        {
            GameObject player = InitPlayer();
            InitPlayerUI();
            InitCamera(player);
        }

        private GameObject InitPlayer()
        {
            Transform spawnPointTransform = GameObject.FindWithTag(Constants.Characters.PlayerInitialPoint).transform;
            return _gameFactory.CreatePlayer(spawnPointTransform);
        }

        private void InitPlayerUI() =>
            _gameFactory.CreatePlayerUI();

        private void InitCamera(GameObject player) =>
            Camera.main?.GetComponent<CameraFollow>().Follow(player);
    }
}