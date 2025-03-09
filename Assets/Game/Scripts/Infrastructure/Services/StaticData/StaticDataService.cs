using System.Collections.Generic;
using System.Linq;
using Game.Scripts.AssetManager;
using Game.Scripts.Logic.OrderLogic;
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
        
        public CustomerData DataForCustomer() =>
            _customerData;
        
        public DishData DataForDish(DishTypeId typeId) =>
            _orders.GetValueOrDefault(typeId);
        
        public CookingAreaData DataForCookingArea(DishTypeId typeId) =>
            _cookingAreas.GetValueOrDefault(typeId);
    }
}