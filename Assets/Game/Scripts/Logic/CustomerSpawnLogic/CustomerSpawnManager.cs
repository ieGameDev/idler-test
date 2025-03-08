using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic.OrderLogic;
using Game.Scripts.Logic.ScriptableObjects;
using UnityEngine;

namespace Game.Scripts.Logic.CustomerSpawnLogic
{
    public class CustomerSpawnManager : MonoBehaviour
    {
        private readonly Queue<OrderTrigger> _freeTriggers = new();
        
        private CustomerSpawner _spawner;
        private GameObject _player;
        private CustomerData _customerData;
        private bool _isSpawning;

        public void Initialize(CustomerSpawner spawner, OrderTrigger[] orderTriggers, GameObject player,
            IStaticDataService staticDataService)
        {
            _player = player;
            _spawner = spawner;
            _customerData = staticDataService.DataForCustomer();

            foreach (OrderTrigger trigger in orderTriggers)
                trigger.OnPlaceAvailable += OnTriggerBecameAvailable;
        }
        
        private void OnTriggerBecameAvailable(GameObject triggerObject)
        {
            OrderTrigger trigger = triggerObject.GetComponent<OrderTrigger>();

            if (!_freeTriggers.Contains(trigger))
                _freeTriggers.Enqueue(trigger);

            if (!_isSpawning)
                StartCoroutine(SpawnCustomersWithDelay());
        }

        private IEnumerator SpawnCustomersWithDelay()
        {
            _isSpawning = true;

            while (_freeTriggers.Count > 0)
            {
                OrderTrigger trigger = _freeTriggers.Dequeue();

                CustomerSpawner randomSpawner = _spawner;
                randomSpawner.SpawnCustomer(trigger, _player);

                yield return new WaitForSeconds(_customerData.SpawnDelay);
            }

            _isSpawning = false;
        }
    }
}