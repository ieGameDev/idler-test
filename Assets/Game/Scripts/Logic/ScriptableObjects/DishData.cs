using Game.Scripts.Logic.OrderLogic;
using UnityEngine;

namespace Game.Scripts.Logic.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DishData", menuName = "ScriptableObjects/DishData")]
    public class DishData : ScriptableObject
    {
        public DishTypeId DishTypeId;
        public string DishName;
        public Sprite DishImage;
        public int Price;
    }
}