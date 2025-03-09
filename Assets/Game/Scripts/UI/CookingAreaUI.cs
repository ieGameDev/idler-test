using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class CookingAreaUI : MonoBehaviour
    {
        [Header("UI Settings")] 
        [SerializeField] private Button _cookingButton;
        [SerializeField] private GameObject _dishImage;
        [SerializeField] private GameObject _fullImage;
        [SerializeField] private Image _dishIcon;

        [Header("Cooking Area Icon Settings")] 
        [SerializeField] private Image _circleImage;
        [SerializeField] private Sprite _unlockedCircle;
        [SerializeField] private Sprite _lockedCircle;

        [Header("Timer Settings")] 
        [SerializeField] protected GameObject _timer;
        [SerializeField] protected Image _progressImage;
        [SerializeField] protected Gradient _progressGradient;

        [Header("Buy Button Settings")] 
        [SerializeField] private Button _buyButton;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private TextMeshProUGUI _buyText;
        [SerializeField] private TextMeshProUGUI _notEnoughText;
        [SerializeField] private Sprite _greenButton;
        [SerializeField] private Sprite _redButton;

        public Button CookingButton => _cookingButton;
        public Image ProgressImage => _progressImage;
        public Gradient ProgressGradient => _progressGradient;
        public Button BuyButton => _buyButton;
        public Image BuyButtonImage => _buyButton.GetComponent<Image>();
        public TextMeshProUGUI Price => _price;
        public TextMeshProUGUI BuyText => _buyText;
        public TextMeshProUGUI NotEnoughText => _notEnoughText;
        public Sprite GreenButton => _greenButton;
        public Sprite RedButton => _redButton;
        public Image CircleImage => _circleImage;
        public Sprite UnlockedCircle => _unlockedCircle;
        public Sprite LockedCircle => _lockedCircle;
        public Image DishIcon => _dishIcon;
        public Sprite UnlockedDishSprite { get; private set; }
        public Sprite LockedDishSprite { get; private set; }

        public void Construct(Sprite unlockedDishSprite, Sprite lockedDishSprite)
        {
            UnlockedDishSprite = unlockedDishSprite;
            LockedDishSprite = lockedDishSprite;
        }

        private void Awake()
        {
            HideCookingButton();
            HideProgressImage();
            HideFullImage();
            HideBuyButton();
            ShowDishImage();
        }

        public void HideBuyButton() => Hide(_buyButton.gameObject);

        public void ShowBuyButton() => Show(_buyButton.gameObject);

        public void HideCookingButton() => Hide(_cookingButton.gameObject);

        public void ShowCookingButton() => Show(_cookingButton.gameObject);

        public void HideDishImage() => Hide(_dishImage);

        public void ShowDishImage() => Show(_dishImage);

        public void HideFullImage() => Hide(_fullImage);

        public void ShowFullImage() => Show(_fullImage);

        public void HideProgressImage() => Hide(_timer);

        public void ShowProgressImage()
        {
           _timer.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        private void Show(GameObject obj)
        {
            obj.transform.localScale = Vector3.one;
        }

        private void Hide(GameObject obj)
        {
            obj.transform.localScale = Vector3.zero;
        }
    }
}