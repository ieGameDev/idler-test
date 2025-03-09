using System;
using System.Collections.Generic;
using Game.Scripts.Data;
using Game.Scripts.Infrastructure.DI;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Logic.OrderLogic;
using Game.Scripts.UI;
using UnityEngine;

namespace Game.Scripts.Logic.CookingLogic
{
    public class BuyCookingArea : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private CookingAreaUI _cookingAreaUI;
        [SerializeField] private DishCookingTrigger _playerTrigger;

        private ISaveLoadService _saveLoadService;
        private MoneyStatus _moneyStatus;
        private DishTypeId _dishType;
        private WorldData _worldData;
        private GameObject _objectToShow;
        private int _price;

        public bool IsUnlocked { get; private set; }

        public void Construct(DishTypeId dishType, bool isUnlocked, int price, WorldData worldData,
            GameObject objectToShow)
        {
            _dishType = dishType;

            UnlockedCookingAreaData unlockedCookingAreaData =
                Array.Find(worldData.CookingAreasData.CookingAreas, area => area.DishType == dishType);

            IsUnlocked = unlockedCookingAreaData?.IsUnlocked ?? isUnlocked;

            _price = price;
            _worldData = worldData;
            _objectToShow = objectToShow;
        }

        private void Awake() =>
            _saveLoadService = DiContainer.Instance.Single<ISaveLoadService>();

        private void Start()
        {
            UpdatePriceText();
            UpdateAreaIcon(IsUnlocked);

            _objectToShow.transform.localScale = IsUnlocked ? Vector3.one : Vector3.zero;

            _worldData.CashData.OnChanged += CheckMoneyStatus;
            _playerTrigger.OnPlayerEnter += PlayerEnter;
            _playerTrigger.OnPlayerExit += PlayerExit;
            _cookingAreaUI.BuyButton.onClick.AddListener(BuyArea);
        }

        private void OnDestroy()
        {
            _worldData.CashData.OnChanged -= CheckMoneyStatus;
            _playerTrigger.OnPlayerEnter -= PlayerEnter;
            _playerTrigger.OnPlayerExit -= PlayerExit;
            _cookingAreaUI.BuyButton.onClick.RemoveListener(BuyArea);
        }

        public void SaveProgress(PlayerProgress progress)
        {
            UnlockedCookingAreaData unlockedCookingAreaData =
                Array.Find(progress.WorldData.CookingAreasData.CookingAreas, area => area.DishType == _dishType);

            if (unlockedCookingAreaData != null)
                unlockedCookingAreaData.IsUnlocked = IsUnlocked;
            else
            {
                UnlockedCookingAreaData newCookingAreaData = new UnlockedCookingAreaData
                    { DishType = _dishType, IsUnlocked = IsUnlocked };
                List<UnlockedCookingAreaData> areasDataList =
                    new List<UnlockedCookingAreaData>(progress.WorldData.CookingAreasData.CookingAreas)
                        { newCookingAreaData };
                progress.WorldData.CookingAreasData.CookingAreas = areasDataList.ToArray();
            }
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (IsUnlocked)
                return;

            UnlockedCookingAreaData unlockedCookingAreaData =
                Array.Find(progress.WorldData.CookingAreasData.CookingAreas, area => area.DishType == _dishType);
            IsUnlocked = unlockedCookingAreaData is { IsUnlocked: true };
        }

        private void CheckMoneyStatus()
        {
            _moneyStatus = _price <= _worldData.CashData.Collected
                ? MoneyStatus.HaveMoney
                : MoneyStatus.NoMoney;

            UpdateUI();
        }

        private void BuyArea()
        {
            if (_moneyStatus == MoneyStatus.NoMoney)
                return;

            _worldData.CashData.Spend(_price);

            IsUnlocked = true;
            HaveMoneyView();
            UnlockedView();
            UpdateAreaIcon(IsUnlocked);
            ShowObject();

            _saveLoadService.SaveProgress();
        }

        private void ShowObject() =>
            _objectToShow.transform.localScale = Vector3.one;

        private void UpdateUI()
        {
            switch (_moneyStatus)
            {
                case MoneyStatus.HaveMoney:
                    HaveMoneyView();
                    return;
                case MoneyStatus.NoMoney:
                    NoMoneyView();
                    return;
                default:
                    HaveMoneyView();
                    return;
            }
        }

        private void PlayerEnter()
        {
            CheckMoneyStatus();

            if (IsUnlocked)
                UnlockedView();
            else
                LockedView();
        }

        private void PlayerExit()
        {
            _cookingAreaUI.HideFullImage();
            _cookingAreaUI.HideBuyButton();
            _cookingAreaUI.HideCookingButton();
            _cookingAreaUI.ShowDishImage();
        }

        private void NoMoneyView()
        {
            _cookingAreaUI.Price.gameObject.SetActive(false);
            _cookingAreaUI.BuyText.gameObject.SetActive(false);
            _cookingAreaUI.NotEnoughText.gameObject.SetActive(true);
            _cookingAreaUI.BuyButton.interactable = false;
            _cookingAreaUI.BuyButtonImage.sprite = _cookingAreaUI.RedButton;
        }

        private void HaveMoneyView()
        {
            _cookingAreaUI.NotEnoughText.gameObject.SetActive(false);
            _cookingAreaUI.Price.gameObject.SetActive(true);
            _cookingAreaUI.BuyText.gameObject.SetActive(true);
            _cookingAreaUI.BuyButton.interactable = true;
            _cookingAreaUI.BuyButtonImage.sprite = _cookingAreaUI.GreenButton;
        }

        private void UnlockedView()
        {
            _cookingAreaUI.HideBuyButton();
            _cookingAreaUI.HideDishImage();

            if (_playerTrigger.PlayerOrder.CanPickUpOrder())
            {
                _cookingAreaUI.HideFullImage();
                _cookingAreaUI.ShowCookingButton();
            }
            else
            {
                _cookingAreaUI.HideCookingButton();
                _cookingAreaUI.ShowFullImage();
            }
        }

        private void LockedView()
        {
            _cookingAreaUI.HideCookingButton();
            _cookingAreaUI.HideDishImage();
            _cookingAreaUI.HideFullImage();
            _cookingAreaUI.ShowBuyButton();
        }

        private void UpdateAreaIcon(bool isUnlocked)
        {
            if (isUnlocked)
            {
                _cookingAreaUI.CircleImage.sprite = _cookingAreaUI.UnlockedCircle;
                _cookingAreaUI.DishIcon.sprite = _cookingAreaUI.UnlockedDishSprite;
            }
            else
            {
                _cookingAreaUI.CircleImage.sprite = _cookingAreaUI.LockedCircle;
                _cookingAreaUI.DishIcon.sprite = _cookingAreaUI.LockedDishSprite;
            }
        }

        private void UpdatePriceText()
        {
            _cookingAreaUI.Price.text = $"${_price}";
            _cookingAreaUI.NotEnoughText.text = $"${_price} needed";
        }
    }
}