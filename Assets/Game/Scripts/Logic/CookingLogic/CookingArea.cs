using Game.Scripts.Logic.OrderLogic;
using UnityEngine;

namespace Game.Scripts.Logic.CookingLogic
{
    public class CookingArea : MonoBehaviour
    {
        [SerializeField] private DishTypeId _dishTypeId;
        [SerializeField] private GameObject _objectToShow;
        
        public DishTypeId DishTypeId => _dishTypeId;
        public GameObject ObjectToShow => _objectToShow;
    }
}