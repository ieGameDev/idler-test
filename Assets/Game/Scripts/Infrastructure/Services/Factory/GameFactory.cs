using System.Collections.Generic;
using Game.Scripts.AssetManager;
using Game.Scripts.Customers.Cash;
using Game.Scripts.Infrastructure.Services.Input;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Infrastructure.Services.UI;
using Game.Scripts.Logic.CookingLogic;
using Game.Scripts.Logic.ObjectPool;
using Game.Scripts.Logic.OrderLogic;
using Game.Scripts.Logic.PurchaseAreaLogic;
using Game.Scripts.Logic.ScriptableObjects;
using Game.Scripts.Player;
using Game.Scripts.UI;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IInputService _inputService;
        private readonly IStaticDataService _staticData;
        private readonly IProgressService _progressService;
        private readonly IWindowService _windowService;

        private GameObject _player;
        private PoolBase<CashItem> _objectPool;

        public List<ISavedProgressReader> ProgressReaders { get; } = new();
        public List<ISavedProgress> ProgressWriters { get; } = new();

        public GameFactory(IInputService inputService, IStaticDataService staticData, IProgressService progressService,
            IWindowService windowService)
        {
            _inputService = inputService;
            _staticData = staticData;
            _progressService = progressService;
            _windowService = windowService;
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

        public GameObject CreatePlayerUI()
        {
            GameObject playerUI = Object.Instantiate(Resources.Load<GameObject>(AssetAddress.Player.HUDPath));
            playerUI.GetComponentInChildren<CashCounter>().Construct(_progressService.Progress.WorldData);

            foreach (OpenWindowButton button in playerUI.GetComponentsInChildren<OpenWindowButton>())
                button.Construct(_windowService);

            RegisterProgressWatchers(playerUI);
            
            return playerUI;
        }

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

        public GameObject CreateCookingArea(CookingArea cookingArea, DishTypeId dishTypeId, GameObject objectToShow)
        {
            CookingAreaData staticData = _staticData.DataForCookingArea(dishTypeId);

            Sprite unlockedDishSprite = staticData.UnlockedDishSprite;
            Sprite lockedDishSprite = staticData.LockedDishSprite;
            float cookingTime = staticData.CookingTime;
            DishTypeId typeId = staticData.DishTypeId;
            bool isUnlocked = staticData.IsUnlocked;
            int price = staticData.UnlockPrice;

            GameObject area = Object.Instantiate(Resources.Load<GameObject>(AssetAddress.OrderPath.CookingAreaPath),
                cookingArea.transform.position,
                cookingArea.transform.rotation,
                cookingArea.transform);

            RegisterProgressWatchers(area);

            area.GetComponentInChildren<CookDish>().Construct(this, typeId, cookingTime);
            area.GetComponentInChildren<CookingAreaUI>().Construct(unlockedDishSprite, lockedDishSprite);
            area.GetComponentInChildren<BuyCookingArea>()
                .Construct(typeId, isUnlocked, price, _progressService.Progress.WorldData, objectToShow);

            return area;
        }

        public GameObject CreatePurchasableArea(PurchasableArea purchasableArea, PurchasableAreaTypeId typeId)
        {
            PurchasableAreaData staticData = _staticData.DataForPurchasableArea(typeId);

            int price = staticData.UnlockPrice;
            Sprite icon = staticData.Icon;

            GameObject area = Object.Instantiate(Resources.Load<GameObject>(AssetAddress.OrderPath.PurchasableAreaPath),
                purchasableArea.transform.position,
                purchasableArea.transform.rotation,
                purchasableArea.transform);

            RegisterProgressWatchers(area);

            area.GetComponentInChildren<BuyPurchasableArea>().Construct(typeId, price, icon,
                _progressService.Progress.WorldData,
                purchasableArea.ObjectsToShow,
                purchasableArea.TriggerPoints,
                purchasableArea.ObjectsToHide);

            return area;
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