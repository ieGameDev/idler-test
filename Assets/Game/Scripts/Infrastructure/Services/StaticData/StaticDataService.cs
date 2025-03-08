using System.Collections.Generic;
using System.Linq;
using Game.Scripts.AssetManager;
using Game.Scripts.Logic.OrderLogic;
using Game.Scripts.Logic.ScriptableObjects;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private CustomerData _customerData;
        private Dictionary<DishTypeId, DishData> _orders;
        
        public void LoadCustomer() =>
            _customerData = Resources.Load<CustomerData>(AssetAddress.DataPath.CustomerDataPath);
        
        public void LoadDishes()
        {
            _orders = Resources.LoadAll<DishData>(AssetAddress.DataPath.DishesDataPath)
                .ToDictionary(x => x.DishTypeId, x => x);
        }
        
        public CustomerData DataForCustomer() =>
            _customerData;
        
        public DishData DataForDish(DishTypeId typeId) =>
            _orders.GetValueOrDefault(typeId);
    }
}