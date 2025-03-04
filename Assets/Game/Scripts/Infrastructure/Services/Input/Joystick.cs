using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Input
{
    public class Joystick : JoystickHandler
    {
        public static Vector2 JoystickAxis { get; private set; }

        private void Update() => 
            JoystickAxis = new Vector2(InputVector.x, InputVector.y);
    }
}