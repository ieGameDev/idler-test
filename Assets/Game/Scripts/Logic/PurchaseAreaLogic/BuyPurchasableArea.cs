using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Data;
using Game.Scripts.Infrastructure.DI;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Logic.CookingLogic;
using Game.Scripts.UI;
using UnityEngine;

namespace Game.Scripts.Logic.PurchaseAreaLogic
{
    public class BuyPurchasableArea : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private PurchaseAreaUI _purchaseAreaUI;
        [SerializeField] private PurchasableTrigger _playerTrigger;

        private ISaveLoadService _saveLoadService;
        private MoneyStatus _moneyStatus;
        private PurchasableAreaTypeId _typeId;
        private WorldData _worldData;
        private List<GameObject> _triggers;
        private GameObject _prefab;
        private GameObject _objectsToDeactivate;
        private GameObject[] _doors;
        private int _price;
        private bool _isUnlocked;

        public void Construct(PurchasableAreaTypeId typeId, int price, Sprite icon, WorldData worldData,
            GameObject prefab, List<GameObject> triggers, GameObject objectsToDeactivate)
        {
            _typeId = typeId;
            _price = price;
            _purchaseAreaUI.IconImage.sprite = icon;
            _worldData = worldData;
            _prefab = prefab;
            _triggers = triggers;
            _objectsToDeactivate = objectsToDeactivate;

            UnlockedSittingAreaData unlockedSittingArea = Array.Find(worldData.SittingAreasData.SittingAreas,
                area => area.PurchasableAreaType == typeId);

            _isUnlocked = unlockedSittingArea?.IsUnlocked ?? false;

            CheckAreaIsUnlocked();
        }

        private void Awake() =>
            _saveLoadService = DiContainer.Instance.Single<ISaveLoadService>();

        private void Start()
        {
            UpdatePriceText();

            _worldData.CashData.OnChanged += CheckMoneyStatus;
            _playerTrigger.OnPlayerEnter += PlayerEnter;
            _playerTrigger.OnPlayerExit += PlayerExit;
            _purchaseAreaUI.BuyButton.onClick.AddListener(BuyArea);
        }

        private void OnDestroy()
        {
            _worldData.CashData.OnChanged -= CheckMoneyStatus;
            _playerTrigger.OnPlayerEnter -= PlayerEnter;
            _playerTrigger.OnPlayerExit -= PlayerExit;
            _purchaseAreaUI.BuyButton.onClick.RemoveListener(BuyArea);
        }

        public void SaveProgress(PlayerProgress progress)
        {
            UnlockedSittingAreaData unlockedSittingArea = Array.Find(progress.WorldData.SittingAreasData.SittingAreas,
                area => area.PurchasableAreaType == _typeId);

            if (unlockedSittingArea != null)
                unlockedSittingArea.IsUnlocked = _isUnlocked;
            else
            {
                UnlockedSittingAreaData newSittingAreaData = new UnlockedSittingAreaData
                {
                    PurchasableAreaType = _typeId, IsUnlocked = _isUnlocked
                };

                List<UnlockedSittingAreaData> areasDataList =
                    new List<UnlockedSittingAreaData>(progress.WorldData.SittingAreasData.SittingAreas)
                    {
                        newSittingAreaData
                    };

                progress.WorldData.SittingAreasData.SittingAreas = areasDataList.ToArray();
            }
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (_isUnlocked)
                return;

            UnlockedSittingAreaData unlockedSittingArea = Array.Find(progress.WorldData.SittingAreasData.SittingAreas,
                area => area.PurchasableAreaType == _typeId);

            _isUnlocked = unlockedSittingArea is { IsUnlocked: true };
        }

        private void CheckAreaIsUnlocked()
        {
            if (_isUnlocked)
                UnlockedView();
            else
                LockedView();
        }

        private void UnlockedView()
        {
            foreach (GameObject trigger in _triggers)
                trigger.gameObject.SetActive(true);

            _prefab.transform.localScale = Vector3.one;
            _objectsToDeactivate.transform.localScale = Vector3.zero;

            gameObject.SetActive(false);
        }

        private void LockedView()
        {
            foreach (GameObject trigger in _triggers)
                trigger.gameObject.SetActive(false);

            _prefab.transform.localScale = Vector3.zero;
            _objectsToDeactivate.transform.localScale = Vector3.one;
        }

        private void BuyArea()
        {
            if (_moneyStatus == MoneyStatus.NoMoney)
                return;

            _worldData.CashData.Spend(_price);

            _isUnlocked = true;
            BuyAreaView();
            UnlockingProcess();

            _saveLoadService.SaveProgress();
        }

        private void UnlockingProcess()
        {
            _purchaseAreaUI.HideAreaImageBackground();
            _purchaseAreaUI.HideTableImage();
            _purchaseAreaUI.HideBuyButton();

            _objectsToDeactivate.transform.localScale = Vector3.zero;
            _prefab.transform.localScale = Vector3.one;

            foreach (GameObject trigger in _triggers)
                trigger.gameObject.SetActive(true);

            StartCoroutine(TurnOffPurchasableArea());
        }

        private void CheckMoneyStatus()
        {
            _moneyStatus = _price <= _worldData.CashData.Collected
                ? MoneyStatus.HaveMoney
                : MoneyStatus.NoMoney;

            UpdateUI();
        }

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
            _purchaseAreaUI.HideAreaImageBackground();
            _purchaseAreaUI.HideTableImage();
            _purchaseAreaUI.ShowBuyButton();

            CheckMoneyStatus();
        }

        private void PlayerExit()
        {
            _purchaseAreaUI.ShowAreaImageBackground();
            _purchaseAreaUI.HideBuyButton();
            _purchaseAreaUI.ShowTableImage();
        }

        private void NoMoneyView()
        {
            _purchaseAreaUI.Price.gameObject.SetActive(false);
            _purchaseAreaUI.BuyText.gameObject.SetActive(false);
            _purchaseAreaUI.NotEnoughText.gameObject.SetActive(true);
            _purchaseAreaUI.BuyButton.interactable = false;
            _purchaseAreaUI.BuyButtonImage.sprite = _purchaseAreaUI.RedButton;
        }

        private void HaveMoneyView()
        {
            _purchaseAreaUI.Price.gameObject.SetActive(true);
            _purchaseAreaUI.BuyText.gameObject.SetActive(true);
            _purchaseAreaUI.NotEnoughText.gameObject.SetActive(false);
            _purchaseAreaUI.BuyButton.interactable = true;
            _purchaseAreaUI.BuyButtonImage.sprite = _purchaseAreaUI.GreenButton;
        }

        private void BuyAreaView()
        {
            _playerTrigger.gameObject.SetActive(false);
            _purchaseAreaUI.Price.gameObject.SetActive(false);
            _purchaseAreaUI.BuyText.gameObject.SetActive(false);
            _purchaseAreaUI.NotEnoughText.gameObject.SetActive(false);
            _purchaseAreaUI.BuyButton.gameObject.SetActive(false);
        }

        private void UpdatePriceText()
        {
            _purchaseAreaUI.Price.text = $"${_price}";
            _purchaseAreaUI.NotEnoughText.text = $"${_price} needed";
        }

        private IEnumerator TurnOffPurchasableArea()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
    }
}