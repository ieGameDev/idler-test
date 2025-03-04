using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Input
{
    public abstract class InputService : IInputService
    {
        public abstract Vector2 Axis { get; }

        protected static Vector2 JoystickInputAxis() => Joystick.JoystickAxis;
    }
}