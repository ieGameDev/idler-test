using System.Collections.Generic;
using Game.Scripts.Infrastructure.DI;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Logic.CookingLogic;
using Game.Scripts.Logic.OrderLogic;
using Game.Scripts.Logic.PurchaseAreaLogic;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        
        GameObject CreatePlayer(Transform spawnPoint);
        GameObject CreateCustomer(Transform transform);
        GameObject CreatePlayerUI();
        GameObject CreateCustomerSpawnManager(int orderTriggersCount);
        GameObject CreateDish(DishTypeId typeId, Order parent);
        GameObject CreateCookingArea(CookingArea cookingArea, DishTypeId dishTypeId, GameObject objectToShow);
        GameObject CreatePurchasableArea(PurchasableArea purchasableArea, PurchasableAreaTypeId typeId);

        void CleanUp();
    }
}