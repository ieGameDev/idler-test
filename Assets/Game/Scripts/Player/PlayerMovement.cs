using Game.Scripts.Data;
using Game.Scripts.Infrastructure.Services.Input;
using Game.Scripts.Infrastructure.Services.Progress;
using UnityEngine;

namespace Game.Scripts.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private float _speed;

        private IInputService _inputService;
        private Camera _camera;

        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
            _camera = Camera.main;
        }

        private void Update() => Move();

        public void SaveProgress(PlayerProgress progress) =>
            progress.WorldData.PlayerPosition = transform.position;

        public void LoadProgress(PlayerProgress progress)
        {
            Vector3 savedPosition = progress.WorldData.PlayerPosition;
            ReplacePlayer(savedPosition);
        }

        private void Move()
        {
            Vector3 movementVector = Vector3.zero;

            if (_inputService.Axis.sqrMagnitude > 0.001f)
            {
                movementVector = _camera.transform.TransformDirection(_inputService.Axis);
                movementVector.y = 0;
                movementVector.Normalize();

                transform.forward = movementVector;
                
                _playerAnimator.MoveAnimation();
            }
            else
                _playerAnimator.StopMoving();

            movementVector += Physics.gravity;
            _characterController.Move(movementVector * (_speed * Time.deltaTime));
        }
        
        private void ReplacePlayer(Vector3 position)
        {
            _characterController.enabled = false;
            transform.position = position;
            _characterController.enabled = true;
        }
    }
}