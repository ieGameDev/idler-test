using Game.Scripts.Data;
using Game.Scripts.Logic.ObjectPool;
using UnityEngine;

namespace Game.Scripts.Customers.Cash
{
    public class CashItem : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _trail;
        [SerializeField] private float _cashMoveSpeed;
        
        private CashValue _cashValue;
        private bool _picked;
        private WorldData _worldData;
        private Transform _player;
        private PoolBase<CashItem> _objectPool;

        public void Construct(WorldData worldData, Transform playerTransform)
        {
            _worldData = worldData;
            _player = playerTransform;
        }

        public void Initialize(CashValue cashValue, PoolBase<CashItem> pool)
        {
            _cashValue = cashValue;
            _objectPool = pool;
        }

        private void Update()
        {
            if (_picked)
                MoveToPlayer();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject != _player.gameObject) return;
            if (_picked) return;

            _picked = true;
            MoveToPlayer();
        }

        public void ResetState() =>
            _picked = false;

        private void MoveToPlayer()
        {
            if (!_player) return;

            Vector3 targetPosition = _player.position + Vector3.up;
            transform.position = Vector3.MoveTowards(
                transform.position, targetPosition, _cashMoveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                CollectCash();
        }

        private void CollectCash()
        {
            _worldData.CashData.Collect(_cashValue);
            _trail.Clear();
            
            _objectPool.Return(this);
        }
    }
}