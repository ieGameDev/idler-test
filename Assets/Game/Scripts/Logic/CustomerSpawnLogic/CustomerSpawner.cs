using Game.Scripts.Customers;
using Game.Scripts.Infrastructure.Services.Factory;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic.OrderLogic;
using Game.Scripts.Logic.ScriptableObjects;
using UnityEngine;

namespace Game.Scripts.Logic.CustomerSpawnLogic
{
    public class CustomerSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _returnPoint;
        
        private IGameFactory _gameFactory;
        private CustomerData _customerData;

        public void Construct(IGameFactory factory, IStaticDataService staticDataService)
        {
            _gameFactory = factory;
            _customerData = staticDataService.DataForCustomer();
        }

        public void SpawnCustomer(OrderTrigger orderTrigger, GameObject player)
        {
            GameObject customer = _gameFactory.CreateCustomer(transform);
            CustomerMovement customerMove = customer.GetComponent<CustomerMovement>();
            
            float waitingTime = _customerData.WaitingTime;
            float eatingTime = _customerData.EatingTime;
            
            customerMove.Construct(orderTrigger.gameObject, _returnPoint);
        }
    }
}