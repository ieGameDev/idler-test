using System;
using Game.Scripts.Logic.OrderLogic;

namespace Game.Scripts.Data
{
    [Serializable]
    public class UnlockedCookingAreaData
    {
        public DishTypeId DishType;
        public bool IsUnlocked;
    }
}