using System;
using System.Collections;
using Game.Scripts.UI;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.Customers
{
    public class CustomerReaction : MonoBehaviour
    {
        [SerializeField] private CustomerMovement _customerMove;
        [SerializeField] private CustomerUI _customerUI;
        [SerializeField] private Canvas _customerInteraptionCanvas;
        [SerializeField] private Canvas _buttonCanvas;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        
        public event Action OnSuccessOrder;
        
        private void Start() => TurnOnCanvas();

        public void OnHappyAnimation()
        {
            _customerMove.ChangeDestination();
            
            if(_navMeshAgent.isStopped)
                _navMeshAgent.isStopped = false;
            
            StartCoroutine(HideSmile());
            
            OnSuccessOrder?.Invoke();
        }

        public void OnSadAnimation()
        {
            _customerMove.ChangeDestination();
            
            if(_navMeshAgent.isStopped)
                _navMeshAgent.isStopped = false;
            
            StartCoroutine(HideSmile());
        }

        private IEnumerator HideSmile()
        {
            yield return new WaitForSeconds(1f);
            _customerUI.HideSmile();
            StartCoroutine(TurnOffCanvas());
        }

        private IEnumerator TurnOffCanvas()
        {
            yield return new WaitForSeconds(0.5f);
            _customerInteraptionCanvas.gameObject.SetActive(false);
            _buttonCanvas.gameObject.SetActive(false);
        }

        private void TurnOnCanvas()
        {
            _customerInteraptionCanvas.gameObject.SetActive(true);
            _buttonCanvas.gameObject.SetActive(true);
        }
    }
}