using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Infrastructure.Services.UI;
using Game.Scripts.Logic.OrderLogic;
using Game.Scripts.Logic.PurchaseAreaLogic;
using Game.Scripts.Logic.ScriptableObjects;
using UnityEngine;
using static Game.Scripts.AssetManager.AssetAddress;

namespace Game.Scripts.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private CustomerData _customerData;
        private Dictionary<DishTypeId, DishData> _orders;
        private Dictionary<DishTypeId, CookingAreaData> _cookingAreas;
        private Dictionary<PurchasableAreaTypeId, PurchasableAreaData> _purchasableAreas;
        private Dictionary<WindowId, WindowConfig> _windowConfigs;
        private PlayerUpgradesData _playerUpgrades;

        public void LoadCustomer() =>
            _customerData = Resources.Load<CustomerData>(DataPath.CustomerDataPath);

        public void LoadDishes()
        {
            _orders = Resources.LoadAll<DishData>(DataPath.DishesDataPath)
                .ToDictionary(x => x.DishTypeId, x => x);
        }

        public void LoadCookingAreas()
        {
            _cookingAreas = Resources.LoadAll<CookingAreaData>(DataPath.CookingAreaDataPath)
                .ToDictionary(x => x.DishTypeId, x => x);
        }

        public void LoadPurchasableAreas()
        {
            _purchasableAreas = Resources.LoadAll<PurchasableAreaData>(DataPath.PurchasableAreaDataPath)
                .ToDictionary(x => x.TypeId, x => x);
        }

        public void LoadWindows()
        {
            _windowConfigs = Resources.Load<WindowStaticData>(DataPath.WindowsConfigPath).WindowConfigs
                .ToDictionary(x => x.WindowID, x => x);
        }

        public void LoadPlayerUpgrades() => 
            _playerUpgrades = Resources.Load<PlayerUpgradesData>(DataPath.PlayerUpgradesPath);

        public CustomerData DataForCustomer() =>
            _customerData;

        public DishData DataForDish(DishTypeId typeId) =>
            _orders.GetValueOrDefault(typeId);

        public CookingAreaData DataForCookingArea(DishTypeId typeId) =>
            _cookingAreas.GetValueOrDefault(typeId);

        public PurchasableAreaData DataForPurchasableArea(PurchasableAreaTypeId typeId) =>
            _purchasableAreas.GetValueOrDefault(typeId);

        public WindowConfig ForWindow(WindowId windowId) =>
            _windowConfigs.GetValueOrDefault(windowId);

        public PlayerUpgradesData ForPlayerUpgrades() =>
            _playerUpgrades;
    }
}