using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.Customers
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CustomerMovement : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _customerAgent;
        [SerializeField] private CustomerAnimator _customerAnimator;

        private GameObject _orderPoint;
        private Transform _returnPoint;
        private bool _orderCompleted;

        public void Construct(GameObject orderPoint, Transform returnPoint)
        {
            _orderPoint = orderPoint;
            _returnPoint = returnPoint;
            _orderCompleted = false;
        }

        private void Update()
        {
            SelectDestination();
            MovingAnimation();
        }

        public void ChangeDestination() =>
            _orderCompleted = true;

        private void SelectDestination()
        {
            if (IsOrderTriggerOutOfReached() && !_orderCompleted)
                _customerAgent.SetDestination(_orderPoint.transform.position);
            else
                _customerAgent.SetDestination(_returnPoint.transform.position);
        }

        private void MovingAnimation()
        {
            if (_customerAgent.velocity.magnitude > 1f && _customerAgent.remainingDistance > _customerAgent.radius)
                _customerAnimator.MoveAnimation();
            else
                _customerAnimator.StopMoving();
        }

        private bool IsOrderTriggerOutOfReached() =>
            Vector3.Distance(_customerAgent.transform.position, _orderPoint.transform.position) > 0f;
    }
}