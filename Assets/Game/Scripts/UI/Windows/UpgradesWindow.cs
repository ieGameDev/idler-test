using Game.Scripts.Data;
using Game.Scripts.Logic.CookingLogic;
using Game.Scripts.Logic.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Windows
{
    public class UpgradesWindow : BaseWindow
    {
        [Header("Reward Ad Settings")]
        [SerializeField] private Button _rewardAdButton;
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private int _reward;
        
        private CashValue _cashValue;
        private MoneyStatus _moneyStatus;
        private PlayerUpgradesData _playerData;
        private int _capacity;
        private int _capacityLevel;

        protected override void OnAwake()
        {
            base.OnAwake();
            _cashValue = new CashValue { Value = _reward };
            _rewardAdButton.onClick.AddListener(OnRewardAdClicked);
        }

        protected override void Initialize()
        {
            UpdateText();
        }

        protected override void SubscribeUpdates()
        {
        }

        protected override void CleanUp()
        {
            _rewardAdButton.onClick.RemoveListener(OnRewardAdClicked);
        }
        
        private void OnRewardAdClicked()
        {
            //Reward Logic
            Progress.WorldData.CashData.Collect(_cashValue);
        }

        private void UpdateText()
        {
            _rewardText.text = $"+{_reward}$";
        }
    }
}