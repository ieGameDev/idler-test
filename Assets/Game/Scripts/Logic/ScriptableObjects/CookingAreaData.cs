using Game.Scripts.Logic.OrderLogic;
using UnityEngine;

namespace Game.Scripts.Logic.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CookingAreaData", menuName = "ScriptableObjects/CookingAreaData")]
    public class CookingAreaData : ScriptableObject
    {
        [Header("Cooking Area Settings")]
        public DishTypeId DishTypeId;
        public float CookingTime;
        public Sprite UnlockedDishSprite;
        public Sprite LockedDishSprite;
        
        [Header("Purchase Settings")]
        public bool IsUnlocked; 
        public int UnlockPrice;
    }
}