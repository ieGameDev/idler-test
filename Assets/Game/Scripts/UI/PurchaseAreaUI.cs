using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class PurchaseAreaUI : MonoBehaviour
    {
        [Header("UI Settings")] 
        [SerializeField] private GameObject _tableImage;

        [Header("Area Image Settings")] 
        [SerializeField] private Image _areaBackground;

        [Header("Buy Button Settings")] 
        [SerializeField] private Button _buyButton;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private TextMeshProUGUI _buyText;
        [SerializeField] private TextMeshProUGUI _notEnoughText;
        [SerializeField] private Sprite _greenButton;
        [SerializeField] private Sprite _redButton;
        
        [Header("Icon Settings")]
        [SerializeField] private Image _iconImage;

        public Button BuyButton => _buyButton;
        public Image BuyButtonImage => _buyButton.GetComponent<Image>();
        public TextMeshProUGUI Price => _price;
        public TextMeshProUGUI BuyText => _buyText;
        public TextMeshProUGUI NotEnoughText => _notEnoughText;
        public Sprite GreenButton => _greenButton;
        public Sprite RedButton => _redButton;
        public Image IconImage => _iconImage;

        private void Awake()
        {
            HideBuyButton();
            ShowTableImage();
        }

        public void HideBuyButton() => Hide(_buyButton.gameObject);

        public void ShowBuyButton() => Show(_buyButton.gameObject);
        
        public void HideTableImage() => Hide(_tableImage);

        public void ShowTableImage() => Show(_tableImage);

        public void HideAreaImageBackground() => Hide(_areaBackground.gameObject);

        public void ShowAreaImageBackground() => 
            _areaBackground.gameObject.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);

        private void Show(GameObject obj) => 
            obj.transform.localScale = Vector3.one;

        private void Hide(GameObject obj) => 
            obj.transform.localScale = Vector3.zero;
    }
}