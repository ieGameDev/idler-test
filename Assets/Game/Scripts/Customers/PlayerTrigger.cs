using System;
using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Customers
{
    public class PlayerTrigger : MonoBehaviour
    {
        public event Action OnPlayerClose;
        public event Action OnPlayerFar;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Constants.Characters.PlayerTag)) 
                return;

            OnPlayerClose?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(Constants.Characters.PlayerTag))
                return;

            OnPlayerFar?.Invoke();
        }
    }
}