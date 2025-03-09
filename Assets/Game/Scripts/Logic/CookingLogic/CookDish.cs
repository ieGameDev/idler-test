using System.Collections;
using Game.Scripts.Infrastructure.Services.Factory;
using Game.Scripts.Logic.OrderLogic;
using Game.Scripts.UI;
using UnityEngine;

namespace Game.Scripts.Logic.CookingLogic
{
    public class CookDish : MonoBehaviour
    {
        [SerializeField] private DishCookingTrigger _dishCookingTrigger;
        [SerializeField] private CookingAreaUI _cookingAreaUI;
        [SerializeField] private BuyCookingArea _buyCookingArea;

        private IGameFactory _gameFactory;
        private DishTypeId _dishType;
        private float _cookingTime;

        public bool IsDishAvailable => _buyCookingArea.IsUnlocked;
        public DishTypeId DishType => _dishType;

        public void Construct(IGameFactory gameFactory, DishTypeId typeId, float cookingTime)
        {
            _gameFactory = gameFactory;
            _dishType = typeId;
            _cookingTime = cookingTime;
        }

        private void Start() =>
            _cookingAreaUI.CookingButton.onClick.AddListener(Cook);

        private void OnDestroy() =>
            _cookingAreaUI.CookingButton.onClick.RemoveListener(Cook);

        private void Cook()
        {
            _cookingAreaUI.HideCookingButton();
            _cookingAreaUI.ShowProgressImage();
            StartCoroutine(CookingProcess());
        }

        private IEnumerator CookingProcess()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _cookingTime)
            {
                if (!_dishCookingTrigger.PlayerCanCooking)
                {
                    _cookingAreaUI.HideProgressImage();
                    yield break;
                }

                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / _cookingTime;

                _cookingAreaUI.ProgressImage.fillAmount = progress;
                _cookingAreaUI.ProgressImage.color = _cookingAreaUI.ProgressGradient.Evaluate(progress);

                yield return null;
            }

            DishIsReady();
        }

        private void DishIsReady()
        {
            Order order = _dishCookingTrigger.PlayerOrder;
            order.PickUpOrder(_gameFactory, _dishType);

            if (order.CanPickUpOrder())
            {
                _cookingAreaUI.HideProgressImage();
                _cookingAreaUI.HideFullImage();
                _cookingAreaUI.ShowCookingButton();
            }
            else
            {
                _cookingAreaUI.HideCookingButton();
                _cookingAreaUI.HideProgressImage();
                _cookingAreaUI.ShowFullImage();
            }
        }
    }
}