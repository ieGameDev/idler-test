using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Scripts.Infrastructure.Services.Input
{
    public abstract class JoystickHandler : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image _joystickArea;
        [SerializeField] private Image _joystickBackground;
        [SerializeField] private Image _joystick;
        [SerializeField] private Canvas _canvas;

        private RectTransform _backgroundRectTransform;
        private RectTransform _joystickRectTransform;
        private Camera _uiCamera;

        protected Vector2 InputVector;

        private void Start()
        {
            _backgroundRectTransform = _joystickBackground.rectTransform;
            _joystickRectTransform = _joystick.rectTransform;

            _uiCamera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera;

            _joystickBackground.gameObject.SetActive(false);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _joystickBackground.rectTransform, eventData.position, _uiCamera, out Vector2 joystickPosition))
                return;

            float scaleFactorX = 2f / (_backgroundRectTransform.sizeDelta.x * _canvas.scaleFactor);
            float scaleFactorY = 2f / (_backgroundRectTransform.sizeDelta.y * _canvas.scaleFactor);

            joystickPosition.x *= scaleFactorX;
            joystickPosition.y *= scaleFactorY;

            InputVector = joystickPosition;
            InputVector = Vector2.ClampMagnitude(InputVector, 1f);

            _joystickRectTransform.anchoredPosition = new Vector2(
                InputVector.x * (_backgroundRectTransform.sizeDelta.x / 2),
                InputVector.y * (_backgroundRectTransform.sizeDelta.y / 2)
            );
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _joystickArea.rectTransform, eventData.position, _uiCamera, out Vector2 localPoint)) 
                return;

            _backgroundRectTransform.anchoredPosition = localPoint / _canvas.scaleFactor;
            _joystickRectTransform.anchoredPosition = Vector2.zero;

            _joystickBackground.gameObject.SetActive(true);

            OnDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            InputVector = Vector2.zero;
            _joystickRectTransform.anchoredPosition = Vector2.zero;

            _joystickBackground.gameObject.SetActive(false);
        }
    }
}