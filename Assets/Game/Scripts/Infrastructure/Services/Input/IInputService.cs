using Game.Scripts.Infrastructure.DI;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Input
{
    public interface IInputService : IService
    {
        Vector2 Axis { get; }
    }
}