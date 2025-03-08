using System.Collections.Generic;
using Game.Scripts.AssetManager;
using Game.Scripts.Customers.Cash;
using Game.Scripts.Infrastructure.Services.Input;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic.ObjectPool;
using Game.Scripts.Logic.OrderLogic;
using Game.Scripts.Logic.ScriptableObjects;
using Game.Scripts.Player;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IInputService _inputService;
        private readonly IStaticDataService _staticData;
        private readonly IProgressService _progressService;

        private GameObject _player;
        private PoolBase<CashItem> _objectPool;

        public List<ISavedProgressReader> ProgressReaders { get; } = new();
        public List<ISavedProgress> ProgressWriters { get; } = new();

        public GameFactory(IInputService inputService, IStaticDataService staticData, IProgressService progressService)
        {
            _inputService = inputService;
            _staticData = staticData;
            _progressService = progressService;
        }

        public GameObject CreatePlayer(Transform spawnPoint)
        {
            _player = Object.Instantiate(Resources.Load<GameObject>(AssetAddress.Player.PlayerPath), spawnPoint);

            PlayerMovement playerMovement = _player.GetComponent<PlayerMovement>();
            Order order = _player.GetComponentInChildren<Order>();

            playerMovement.Construct(_inputService);
            order.Construct(2);

            RegisterProgressWatchers(_player);

            return _player;
        }

        public void CreatePlayerUI() =>
            Object.Instantiate(Resources.Load<GameObject>(AssetAddress.Player.HUDPath));

        public GameObject CreateCustomerSpawnManager(int orderTriggersCount)
        {
            _objectPool = new PoolBase<CashItem>(PreloadLootItem, GetAction, ReturnAction, orderTriggersCount);
            return Object.Instantiate(Resources.Load<GameObject>(AssetAddress.Customers.CustomerSpawnManagerPath));
        }

        public GameObject CreateCustomer(Transform transform)
        {
            CustomerData staticData = _staticData.DataForCustomer();
            List<GameObject> customerPrefabs = staticData.CustomerPrefabs;
            GameObject randomCustomer = customerPrefabs[Random.Range(0, customerPrefabs.Count)];

            GameObject customer = Object.Instantiate(randomCustomer, transform);
            
            CashSpawner cashSpawner = customer.GetComponentInChildren<CashSpawner>();
            cashSpawner.Construct(_objectPool);

            return customer;
        }

        public GameObject CreateDish(DishTypeId typeId, Order parent)
        {
            DishData staticData = _staticData.DataForDish(typeId);

            Sprite dishImage = staticData.DishImage;
            string dishName = staticData.DishName;
            DishTypeId dishType = staticData.DishTypeId;
            int price = staticData.Price;

            GameObject dish = Object.Instantiate(Resources.Load<GameObject>(AssetAddress.OrderPath.DishPath),
                parent.transform.position,
                parent.transform.rotation,
                parent.transform);

            dish.GetComponent<Dish>().Construct(dishImage, dishName, dishType, price);

            return dish;
        }

        private CashItem CreateCash()
        {
            GameObject cash = Object.Instantiate(Resources.Load<GameObject>(AssetAddress.OrderPath.CashPath));
            RegisterProgressWatchers(cash);

            CashItem cashItem = cash.GetComponent<CashItem>();
            cashItem.Construct(_progressService.Progress.WorldData, _player.transform);

            return cashItem;
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
        
        private CashItem PreloadLootItem() =>
            CreateCash();

        private void GetAction(CashItem cash)
        {
            cash.gameObject.SetActive(true);
            cash.ResetState();
        }

        private void ReturnAction(CashItem cash) => 
            cash.gameObject.SetActive(false);
    }
}