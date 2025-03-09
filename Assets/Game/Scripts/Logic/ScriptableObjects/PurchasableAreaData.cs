using Game.Scripts.Logic.PurchaseAreaLogic;
using UnityEngine;

namespace Game.Scripts.Logic.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PurchasableAreaData", menuName = "ScriptableObjects/PurchasableAreaData")]
    public class PurchasableAreaData : ScriptableObject
    {
        public PurchasableAreaTypeId TypeId;
        public int UnlockPrice;
        public Sprite Icon;
    }
}