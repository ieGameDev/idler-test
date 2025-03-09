using System.Collections.Generic;
using Game.Scripts.Customers;
using Game.Scripts.Infrastructure.Services.Factory;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic.CookingLogic;
using Game.Scripts.Logic.OrderLogic;
using Game.Scripts.Logic.ScriptableObjects;
using Game.Scripts.UI;
using UnityEngine;

namespace Game.Scripts.Logic.CustomerSpawnLogic
{
    public class CustomerSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _returnPoint;
        
        private IGameFactory _gameFactory;
        private List<CookDish> _cookingAreas;
        private CustomerData _customerData;

        public void Construct(IGameFactory factory, IStaticDataService staticDataService, List<CookDish> cookingAreas)
        {
            _gameFactory = factory;
            _customerData = staticDataService.DataForCustomer();
            _cookingAreas = cookingAreas;
        }

        public void SpawnCustomer(OrderTrigger orderTrigger, GameObject player)
        {
            GameObject customer = _gameFactory.CreateCustomer(transform);
            CustomerMovement customerMove = customer.GetComponent<CustomerMovement>();
            CustomerUI customerUI = customer.GetComponentInChildren<CustomerUI>();
            CustomerBehavior customerBehavior = customer.GetComponentInChildren<CustomerBehavior>();
            Order order = customer.GetComponentInChildren<Order>();
            
            order.CreateOrder(_gameFactory, _cookingAreas);
            
            float waitingTime = _customerData.WaitingTime;
            float eatingTime = _customerData.EatingTime;
            
            customerMove.Construct(orderTrigger.gameObject, _returnPoint);
            customerUI.Construct(orderTrigger);
            customerBehavior.Construct(orderTrigger, player, waitingTime, eatingTime);
        }
    }
}