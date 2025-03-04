using Game.Scripts.Infrastructure.Services.Input;
using UnityEngine;

namespace Game.Scripts.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _speed;

        private IInputService _inputService;
        private Camera _camera;

        private void Start()
        {
            _inputService = new DesktopInput();
            _camera = Camera.main;
        }

        private void Update() => Move();

        private void Move()
        {
            Vector3 movementVector = Vector3.zero;

            if (_inputService.Axis.sqrMagnitude > 0.001f)
            {
                movementVector = _camera.transform.TransformDirection(_inputService.Axis);
                movementVector.y = 0;
                movementVector.Normalize();

                transform.forward = movementVector;
            }

            movementVector += Physics.gravity;
            _characterController.Move(movementVector * (_speed * Time.deltaTime));
        }
    }
}