using System;
using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Logic.OrderLogic
{
    public class OrderTrigger : MonoBehaviour
    {
        public event Action<GameObject> OnPlaceAvailable;
        public event Action OnReadyForOrder;
        
        [SerializeField] private GameObject _chair;
        [SerializeField] private Transform _exitPoint;
        
        private bool _placeIsOccupied;

        public GameObject Chair => _chair;
        public Transform ExitPoint => _exitPoint;
        
        private void Start()
        {
            OnPlaceAvailable?.Invoke(gameObject);
            _placeIsOccupied = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Constants.Characters.CustomerTag))
                return;

            if (_placeIsOccupied)
                return;

            _placeIsOccupied = true;
            OnReadyForOrder?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(Constants.Characters.CustomerTag))
                return;

            _placeIsOccupied = false;
        }

        public void InvokeOnPlaceAvailable(GameObject obj) =>
            OnPlaceAvailable?.Invoke(obj);
    }
}