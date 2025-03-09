using System.Collections.Generic;
using Game.Scripts.Logic.OrderLogic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class CustomerUI : MonoBehaviour
    {
        [Header("Order Settings")] 
        [SerializeField] private Button _orderButton;
        [SerializeField] private GameObject _orderList;
        
        [Header("Timer Settings")] 
        [SerializeField] protected GameObject _timer;
        [SerializeField] protected Image _progressImage;
        [SerializeField] protected Gradient _progressGradient;

        [Header("Customer Reaction Settings")]
        [SerializeField] private GameObject _attentionSymbol;
        [SerializeField] private Image _smile;
        [SerializeField] private Sprite _angry;
        [SerializeField] private Sprite _happy;
        [SerializeField] private Canvas _orderListCanvas;
        [SerializeField] private GameObject _eatingAnimation;
        
        [Header("Canvas List")]
        [SerializeField] private List<Canvas> _canvas;

        private OrderTrigger _orderTrigger;

        public Button OrderButton => _orderButton;
        public Image ProgressImage => _progressImage;
        public Gradient ProgressGradient => _progressGradient;
        public List<Canvas> Canvas => _canvas;

        public void Construct(OrderTrigger orderTrigger)
        {
            _orderTrigger = orderTrigger;
            _orderTrigger.OnReadyForOrder += ShowAttention;
            _orderTrigger.OnReadyForOrder += ShowProgressImage;
        }

        private void Awake()
        {
            HideAttention();
            HideOrderButton();
            HideProgressImage();
            HideOrderList();
            HideSmile();
            HideEatingAnimation();
            IncreaseOrderListCanvas();
        }

        private void OnDestroy()
        {
            _orderTrigger.OnReadyForOrder -= ShowAttention;
            _orderTrigger.OnReadyForOrder -= ShowProgressImage;
        }

        public void HideAttention() => Hide(_attentionSymbol);

        public void ShowAttention() => Show(_attentionSymbol);
        
        public void HideEatingAnimation() => Hide(_eatingAnimation);
        
        public void ShowEatingAnimation() => Show(_eatingAnimation);

        public void HideOrderButton() => Hide(_orderButton.gameObject);

        public void ShowOrderButton() => Show(_orderButton.gameObject);

        public void HideOrderList() => Hide(_orderList.gameObject);

        public void ShowOrderList() => Show(_orderList.gameObject);
        
        public void IncreaseOrderListCanvas() => 
            _orderListCanvas.transform.localScale = Vector3.one;

        public void DecreaseOrderListCanvas() => 
            _orderListCanvas.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);

        public void ShowSmile(bool successOrder)
        {
            _smile.sprite = successOrder ? _happy : _angry;
            Show(_smile.gameObject);
        }

        public void HideSmile() => Hide(_smile.gameObject);

        public void HideProgressImage() => Hide(_timer);

        public void DecreaseProgressImage() => 
            _timer.transform.localScale = new Vector3(0.23f, 0.23f, 0.23f);

        public void ShowProgressImage() => 
            _timer.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

        private void Show(GameObject obj) => 
            obj.transform.localScale = Vector3.one;

        private void Hide(GameObject obj) => 
            obj.transform.localScale = Vector3.zero;
    }
}