using System;
using Game.Scripts.Logic.PurchaseAreaLogic;

namespace Game.Scripts.Data
{
    [Serializable]
    public class UnlockedSittingAreaData
    {
        public PurchasableAreaTypeId PurchasableAreaType;
        public bool IsUnlocked;
    }
}