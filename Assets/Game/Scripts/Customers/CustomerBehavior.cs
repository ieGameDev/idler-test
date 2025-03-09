using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Logic.OrderLogic;
using Game.Scripts.UI;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.Customers
{
    public class CustomerBehavior : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private CustomerAnimator _customerAnimator;
        [SerializeField] private CustomerReaction _customerReaction;
        [SerializeField] private PlayerTrigger _playerTrigger;
        [SerializeField] private CustomerMovement _customerMove;
        [SerializeField] private CustomerUI _customerUI;
        [SerializeField] private Collider _triggerCollider;
        [SerializeField] private Order _order;

        private const float WaitingForCompareOrderTime = 0.3f;
        private const float WaitingForNewCustomerTime = 2f;

        private OrderTrigger _orderTrigger;
        private GameObject _player;
        private Order _playerOrder;
        private Coroutine _waitingForOrderCoroutine;
        private bool _playerAcceptedOrder;
        private float _waitingTime;
        private float _eatingTime;
        private bool _isComparingOrders;

        public void Construct(OrderTrigger orderTrigger, GameObject player, float waitingTime, float eatingTime)
        {
            _waitingTime = waitingTime;
            _eatingTime = eatingTime;
            _orderTrigger = orderTrigger;
            _player = player;
            _playerOrder = _player.GetComponentInChildren<Order>();
        }

        private void Start()
        {
            TurnOffOrderCollider();
            _customerUI.OrderButton.onClick.AddListener(StartOrder);
            _orderTrigger.OnReadyForOrder += TurnOnOrderCollider;
            _orderTrigger.OnReadyForOrder += WaitingForService;
            _playerTrigger.OnPlayerClose += PlayerIsClose;
            _playerTrigger.OnPlayerFar += PlayerIsFar;
        }

        private void OnDestroy()
        {
            _customerUI.OrderButton.onClick.RemoveListener(StartOrder);
            _orderTrigger.OnReadyForOrder -= TurnOnOrderCollider;
            _orderTrigger.OnReadyForOrder -= WaitingForService;
            _playerTrigger.OnPlayerClose -= PlayerIsClose;
            _playerTrigger.OnPlayerFar -= PlayerIsFar;
        }

        private void PlayerIsClose()
        {
            if (_playerAcceptedOrder)
            {
                CompareCustomerAndPlayerDishes();
                _customerUI.IncreaseOrderListCanvas();
            }
            else
            {
                _customerUI.HideAttention();
                _customerUI.ShowOrderButton();
                _customerUI.DecreaseProgressImage();
            }
        }

        private void PlayerIsFar()
        {
            if (_playerAcceptedOrder)
                _customerUI.DecreaseOrderListCanvas();
            else
            {
                _customerUI.HideOrderButton();
                _customerUI.ShowAttention();
                _customerUI.ShowProgressImage();
            }
        }

        private void CompareCustomerAndPlayerDishes()
        {
            if (_isComparingOrders)
                return;

            _isComparingOrders = true;
            CompareDishes();
        }

        private void CloseOrderProcess()
        {
            _customerUI.HideAttention();
            _customerUI.HideOrderButton();
            _customerUI.HideProgressImage();
            _customerUI.HideOrderList();


            if (_orderTrigger.TriggerType == TriggerType.SeatedPlace)
            {
                if (_waitingForOrderCoroutine != null)
                    StopCoroutine(_waitingForOrderCoroutine);

                StartCoroutine(WaitingWhileCustomerEating());
            }
            else
                OrderClosed(true);
        }

        private void WaitingForService()
        {
            if (_waitingForOrderCoroutine != null)
                return;

            _waitingForOrderCoroutine = StartCoroutine(WaitingForOrder());
            TurnToFood();
        }

        private void StartOrder()
        {
            _playerAcceptedOrder = true;

            _customerUI.HideAttention();
            _customerUI.HideOrderButton();
            _customerUI.ShowOrderList();
            _customerUI.ShowProgressImage();

            if (_waitingForOrderCoroutine != null)
                StopCoroutine(_waitingForOrderCoroutine);

            _waitingForOrderCoroutine = StartCoroutine(WaitingForOrder());
            StartCoroutine(WaitingForCompareOrder());
        }

        private void OrderClosed(bool success)
        {
            TurnOffOrderCollider();
            TurnOffCanvas();

            if (_orderTrigger.TriggerType == TriggerType.SeatedPlace)
            {
                _navMeshAgent.Warp(_orderTrigger.ExitPoint.position);
                _navMeshAgent.ResetPath();
                _navMeshAgent.isStopped = true;
                _customerAnimator.StopSitting();
            }

            if (success)
            {
                _customerAnimator.HappyAnimation();

                if (_orderTrigger.TriggerType == TriggerType.StandingPlace)
                    StopCoroutine(_waitingForOrderCoroutine);
            }
            else
                _customerAnimator.SadAnimation();

            _customerUI.ShowSmile(success);
            StartCoroutine(WaitingForNewCustomer());
        }

        private void TurnToFood()
        {
            if (_orderTrigger.TriggerType == TriggerType.StandingPlace)
                return;
            
            _customerAnimator.SittingAnimation();
            StartCoroutine(WaitingForCustomerToStop());
        }

        private void TurnOffCanvas()
        {
            foreach (Canvas canvas in _customerUI.Canvas)
                canvas.enabled = false;
        }

        private void TurnOnOrderCollider() => _triggerCollider.enabled = true;

        private void TurnOffOrderCollider() => _triggerCollider.enabled = false;

        private void CompareDishes()
        {
            Dictionary<DishTypeId, int> customerDishCounts = new Dictionary<DishTypeId, int>();

            foreach (DishTypeId dish in _order.DishesId)
            {
                if (customerDishCounts.TryGetValue(dish, out int count))
                    customerDishCounts[dish] = count + 1;
                else
                    customerDishCounts[dish] = 1;
            }

            foreach (KeyValuePair<DishTypeId, int> pair in customerDishCounts)
            {
                DishTypeId dishType = pair.Key;
                int customerDishCount = pair.Value;
                int playerDishCount = 0;

                foreach (DishTypeId dish in _playerOrder.DishesId)
                {
                    if (dish == dishType)
                        playerDishCount++;
                }

                int dishesToRemove = Mathf.Min(playerDishCount, customerDishCount);

                _playerOrder.RemoveDishes(dishType, dishesToRemove, null);
                _order.RemoveDishes(dishType, dishesToRemove, CloseOrderProcess);
            }

            _isComparingOrders = false;
        }

        private IEnumerator WaitingWhileCustomerEating()
        {
            _customerUI.ShowEatingAnimation();

            yield return new WaitForSeconds(_eatingTime);

            _customerUI.HideEatingAnimation();
            OrderClosed(true);
        }

        private IEnumerator WaitingForOrder()
        {
            float timer = 0f;

            while (timer < _waitingTime)
            {
                timer += Time.deltaTime;

                _customerUI.ProgressImage.fillAmount = 1 - (timer / _waitingTime);
                _customerUI.ProgressImage.color = _customerUI.ProgressGradient.Evaluate(1 - (timer / _waitingTime));

                yield return null;
            }

            OrderClosed(false);
        }

        private IEnumerator WaitingForCompareOrder()
        {
            yield return new WaitForSeconds(WaitingForCompareOrderTime);
            CompareCustomerAndPlayerDishes();
        }

        private IEnumerator WaitingForNewCustomer()
        {
            yield return new WaitForSeconds(WaitingForNewCustomerTime);
            _orderTrigger.InvokeOnPlaceAvailable(_orderTrigger.gameObject);
        }

        private IEnumerator WaitingForCustomerToStop()
        {
            while (_navMeshAgent.pathPending || _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
                yield return null;

            _navMeshAgent.isStopped = true;

            Quaternion targetRotation = _orderTrigger.Chair.transform.rotation * Quaternion.Euler(0, 180, 0);
            StartCoroutine(SmoothTurnToTarget(targetRotation, 0.2f));
        }

        private IEnumerator SmoothTurnToTarget(Quaternion targetRotation, float duration)
        {
            Quaternion initialRotation = transform.rotation;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetRotation;
        }
    }
}