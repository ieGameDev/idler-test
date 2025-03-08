using Game.Scripts.Data;
using Game.Scripts.Logic.ObjectPool;
using UnityEngine;

namespace Game.Scripts.Customers.Cash
{
    public class CashSpawner : MonoBehaviour
    {
        [SerializeField] private CustomerReaction _customerReaction;
        
        private PoolBase<CashItem> _objectPool;
        private int _cashValue;
        
        public void Construct(PoolBase<CashItem> objectPool) => 
            _objectPool = objectPool;

        private void Start() =>
            _customerReaction.OnSuccessOrder += SpawnCash;

        private void OnDestroy() => 
            _customerReaction.OnSuccessOrder -= SpawnCash;

        public void SetLoot(int cashValue) =>
            _cashValue = cashValue;

        private void SpawnCash()
        {
            CashItem cash = _objectPool.Get();
            cash.transform.position = transform.position + Vector3.up;

            CashValue item = new CashValue { Value = _cashValue };
            cash.Initialize(item, _objectPool);
        }
    }
}