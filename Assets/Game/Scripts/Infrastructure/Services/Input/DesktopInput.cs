using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Input
{
    public class DesktopInput : InputService
    {
        public override Vector2 Axis
        {
            get
            {
                Vector2 axis = JoystickInputAxis();

                if (axis == Vector2.zero)
                    axis = DesktopInputAxis();

                return axis;
            }
        }

        private static Vector2 DesktopInputAxis() => new(UnityEngine.Input.GetAxisRaw("Horizontal"),
            UnityEngine.Input.GetAxisRaw("Vertical"));
    }
}