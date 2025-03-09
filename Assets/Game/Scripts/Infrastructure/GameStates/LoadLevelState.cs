using System.Collections.Generic;
using Game.Scripts.Infrastructure.Services.Factory;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic;
using Game.Scripts.Logic.CookingLogic;
using Game.Scripts.Logic.CustomerSpawnLogic;
using Game.Scripts.Logic.OrderLogic;
using Game.Scripts.Logic.PurchaseAreaLogic;
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
        private readonly IUIFactory _uiFactory;

        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, IGameFactory gameFactory,
            IProgressService progressService, IStaticDataService staticData, IUIFactory uiFactory)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticData = staticData;
            _uiFactory = uiFactory;
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
            InitUIRoot();
            InitGameWorld();
            InformProgressReaders();

            _stateMachine.Enter<GameLoopState>();
        }

        private void InitUIRoot() => _uiFactory.CreateUIRoot();

        private void InitGameWorld()
        {
            List<CookDish> cookingAreas = InitializingCookingArea();
            OrderTrigger[] orderTriggers = InitializingOrderTriggers();
            GameObject player = InitPlayer();
            InitPurchasableArea();
            InitPlayerUI();
            InitCamera(player);
            CustomerSpawner spawner = InitCustomerSpawner(cookingAreas);
            InitCustomerSpawnLogic(spawner, orderTriggers, player);
        }

        private List<CookDish> InitializingCookingArea()
        {
            CookingArea[] areas = Object.FindObjectsByType<CookingArea>(FindObjectsSortMode.None);

            List<GameObject> cookingAreas = new();
            List<CookDish> cookingDishes = new();

            foreach (CookingArea cookingArea in areas)
                cookingAreas.Add(_gameFactory.CreateCookingArea(cookingArea, cookingArea.DishTypeId,
                    cookingArea.ObjectToShow));

            foreach (GameObject area in cookingAreas)
                cookingDishes.Add(area.GetComponentInChildren<CookDish>());

            return cookingDishes;
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

        private CustomerSpawner InitCustomerSpawner(List<CookDish> cookingAreas)
        {
            CustomerSpawner spawner = Object.FindFirstObjectByType<CustomerSpawner>();
            spawner.Construct(_gameFactory, _staticData, cookingAreas);

            return spawner;
        }

        private void InitCustomerSpawnLogic(CustomerSpawner spawner, OrderTrigger[] orderTriggers, GameObject player)
        {
            GameObject customerSpawnManager = _gameFactory.CreateCustomerSpawnManager(orderTriggers.Length);
            CustomerSpawnManager manager = customerSpawnManager.GetComponent<CustomerSpawnManager>();

            manager.Initialize(spawner, orderTriggers, player, _staticData);
        }

        private void InitPurchasableArea()
        {
            PurchasableArea[] areas = Object.FindObjectsByType<PurchasableArea>(FindObjectsSortMode.None);

            foreach (PurchasableArea area in areas)
                _gameFactory.CreatePurchasableArea(area, area.AreaTypeId);
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }
    }
}