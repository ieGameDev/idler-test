using System.Collections.Generic;
using Game.Scripts.Infrastructure.DI;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Logic.OrderLogic;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        
        GameObject CreatePlayer(Transform spawnPoint);
        GameObject CreateCustomer(Transform transform);
        void CreatePlayerUI();
        GameObject CreateCustomerSpawnManager(int orderTriggersCount);
        GameObject CreateDish(DishTypeId typeId, Order parent);

        void CleanUp();
    }
}