using System;
using System.Collections.Generic;
using Game.Scripts.Customers.Cash;
using Game.Scripts.Infrastructure.Services.Factory;
using Game.Scripts.Logic.CookingLogic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Logic.OrderLogic
{
    public class Order : MonoBehaviour
    {
        [SerializeField] private CashSpawner _cashSpawner;

        private int _maxPlayerCapacity;

        public List<DishTypeId> DishesId { get; } = new();

        public void Construct(int maxPlayerCapacity) =>
            _maxPlayerCapacity = maxPlayerCapacity;

        public void CreateOrder(IGameFactory gameFactory, List<CookDish> availableCookDishes)
        {
            int numberOfDishes = Random.Range(1, 3);
            int orderPrice = 0;

            List<DishTypeId> unlockedDishes = new List<DishTypeId>();

            foreach (CookDish cookDish in availableCookDishes)
                if (cookDish.IsDishAvailable)
                    unlockedDishes.Add(cookDish.DishType);

            for (int i = 0; i < numberOfDishes; i++)
            {
                int randomIndex = Random.Range(0, unlockedDishes.Count);
                DishTypeId randomDishType = unlockedDishes[randomIndex];
                GameObject dishPrefab = gameFactory.CreateDish(randomDishType, this);
                Dish dish = dishPrefab.GetComponentInChildren<Dish>();
                DishesId.Add(dish.DishType);

                orderPrice += dish.Price;
            }

            _cashSpawner.SetLoot(orderPrice);
        }

        public void PickUpOrder(IGameFactory gameFactory, DishTypeId typeId)
        {
            if (!CanPickUpOrder())
                return;

            GameObject dish = gameFactory.CreateDish(typeId, this);
            DishesId.Add(dish.GetComponentInChildren<Dish>().DishType);
        }

        public void RemoveDishes(DishTypeId dishId, int count, Action onOrderCompleted)
        {
            Dish[] dishes = GetComponentsInChildren<Dish>();
            int removedCount = 0;

            foreach (Dish dish in dishes)
            {
                if (removedCount >= count)
                    break;

                if (dish.DishType != dishId)
                    continue;

                removedCount++;
                DishRemoveAnimation(dish);
                DishesId.Remove(dishId);
            }

            if (DishesId.Count == 0)
                onOrderCompleted?.Invoke();
        }

        public bool CanPickUpOrder() =>
            DishesId.Count < _maxPlayerCapacity;

        private static void DishRemoveAnimation(Dish dish)
        {
            if (!dish || !dish.transform)
                return;

            if (dish)
                Destroy(dish.gameObject);
        }
    }
}