using UnityEngine;

namespace Game.Scripts.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");

        public void MoveAnimation() =>
            _animator.SetBool(IsRunning, true);

        public void StopMoving() => 
            _animator.SetBool(IsRunning, false);
    }
}