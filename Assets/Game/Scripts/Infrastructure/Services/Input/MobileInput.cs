using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Input
{
    public class MobileInput : InputService
    {
        public override Vector2 Axis => JoystickInputAxis();
    }
}