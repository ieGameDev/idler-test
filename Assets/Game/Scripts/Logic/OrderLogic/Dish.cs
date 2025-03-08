using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Logic.OrderLogic
{
    public class Dish : MonoBehaviour
    {
        [SerializeField] private Image _dishImage;
        [SerializeField] private TextMeshProUGUI _dishName;
        [SerializeField] private TextMeshProUGUI _price;
        
        private DishTypeId _dishType;
        
        public DishTypeId DishType => _dishType;
        public int Price { get; private set; }

        public void Construct(Sprite sprite, string text, DishTypeId typeId, int price)
        {
            _dishImage.sprite = sprite;
            _dishName.text = text;
            _price.text = $"${price.ToString()}";
            _dishType = typeId;
            Price = price;
        }
    }
}