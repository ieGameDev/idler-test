using Game.Scripts.Infrastructure.DI;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Factory
{
    public interface IGameFactory : IService
    {
        GameObject CreatePlayer(Transform spawnPoint);
        GameObject CreatePlayerUI();
    }
}