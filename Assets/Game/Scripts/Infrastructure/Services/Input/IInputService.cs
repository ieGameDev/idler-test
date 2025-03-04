using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Input
{
    public interface IInputService
    {
        Vector2 Axis { get; }
    }
}