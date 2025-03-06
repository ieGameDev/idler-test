using Game.Scripts.Infrastructure.Services.Factory;
using Game.Scripts.Infrastructure.Services.Progress;
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
        private readonly IProgressService _progressService;

        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, IGameFactory gameFactory,
            IProgressService progressService)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _progressService = progressService;
        }

        public void Enter()
        {
            _gameFactory.CleanUp();
            _sceneLoader.LoadScene(Constants.Levels.Main, OnLoaded);
        }

        public void Exit()
        {
        }

        private void OnLoaded()
        {
            InitGameWorld();
            InformProgressReaders();

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

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }
    }
}