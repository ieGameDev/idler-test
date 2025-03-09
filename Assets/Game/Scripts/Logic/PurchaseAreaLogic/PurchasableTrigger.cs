using System;
using UnityEngine;
using static Game.Scripts.Utils.Constants.Characters;

namespace Game.Scripts.Logic.PurchaseAreaLogic
{
    public class PurchasableTrigger : MonoBehaviour
    {
        public event Action OnPlayerEnter;
        public event Action OnPlayerExit;
        
        private GameObject _player;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(PlayerTag))
                return;

            if (!_player) 
                _player = other.gameObject;
            
            OnPlayerEnter?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(PlayerTag))
                return;
            
            OnPlayerExit?.Invoke();
        }
    }
}