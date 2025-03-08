using UnityEngine;

namespace Game.Scripts.Customers
{
    [RequireComponent(typeof(Animator))]
    public class CustomerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsSitting = Animator.StringToHash("IsSitting");

        public void MoveAnimation() =>
            _animator.SetBool(IsMoving, true);

        public void StopMoving() =>
            _animator.SetBool(IsMoving, false);

        public void SittingAnimation() =>
            _animator.SetBool(IsSitting, true);

        public void StopSitting() =>
            _animator.SetBool(IsSitting, false);
    }
}