using System;
using Game.Scripts.Logic.OrderLogic;
using UnityEngine;
using static Game.Scripts.Utils.Constants.Characters;

namespace Game.Scripts.Logic.CookingLogic
{
    public class DishCookingTrigger : MonoBehaviour
    {
        public event Action OnPlayerEnter;
        public event Action OnPlayerExit;

        private GameObject _player;

        public Order PlayerOrder { get; private set; }
        public bool PlayerCanCooking { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(PlayerTag))
                return;

            PlayerCanCooking = true;

            if (!_player) 
                _player = other.gameObject;
            if (!PlayerOrder) 
                PlayerOrder = _player.GetComponentInChildren<Order>();
            
            OnPlayerEnter?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(PlayerTag))
                return;

            PlayerCanCooking = false;
            OnPlayerExit?.Invoke();
        }
    }
}