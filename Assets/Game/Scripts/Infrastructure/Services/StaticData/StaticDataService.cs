using Game.Scripts.AssetManager;
using Game.Scripts.Logic.ScriptableObjects;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private CustomerData _customerData;
        
        public void LoadCustomer() =>
            _customerData = Resources.Load<CustomerData>(AssetAddress.DataPath.CustomerDataPath);
        
        public CustomerData DataForCustomer() =>
            _customerData;
    }
}