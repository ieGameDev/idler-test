using Game.Scripts.Infrastructure.Services.Factory;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic;
using Game.Scripts.Logic.CustomerSpawnLogic;
using Game.Scripts.Logic.OrderLogic;
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
        private readonly IStaticDataService _staticData;

        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, IGameFactory gameFactory,
            IProgressService progressService, IStaticDataService staticData)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticData = staticData;
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
            OrderTrigger[] orderTriggers = InitializingOrderTriggers();
            GameObject player = InitPlayer();
            InitPlayerUI();
            InitCamera(player);
            CustomerSpawner spawner = InitCustomerSpawner();
            InitCustomerSpawnLogic(spawner, orderTriggers, player);
        }

        private OrderTrigger[] InitializingOrderTriggers() =>
            Object.FindObjectsByType<OrderTrigger>(FindObjectsSortMode.None);

        private GameObject InitPlayer()
        {
            Transform spawnPointTransform = GameObject.FindWithTag(Constants.Characters.PlayerInitialPoint).transform;
            return _gameFactory.CreatePlayer(spawnPointTransform);
        }

        private void InitPlayerUI() =>
            _gameFactory.CreatePlayerUI();

        private void InitCamera(GameObject player) =>
            Camera.main?.GetComponent<CameraFollow>().Follow(player);

        private CustomerSpawner InitCustomerSpawner()
        {
            CustomerSpawner spawner = Object.FindFirstObjectByType<CustomerSpawner>();
            spawner.Construct(_gameFactory, _staticData);

            return spawner;
        }

        private void InitCustomerSpawnLogic(CustomerSpawner spawner, OrderTrigger[] orderTriggers, GameObject player)
        {
            GameObject customerSpawnManager = _gameFactory.CreateCustomerSpawnManager(orderTriggers.Length);
            CustomerSpawnManager manager = customerSpawnManager.GetComponent<CustomerSpawnManager>();

            manager.Initialize(spawner, orderTriggers, player, _staticData);
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }
    }
}