using Game.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Windows
{
    public class UpgradesWindow : BaseWindow
    {
        [Header("Capacity Settings")]
        [SerializeField] private Button _capacityButton;
        [SerializeField] private TextMeshProUGUI _capacityPrice;
        [SerializeField] private TextMeshProUGUI _capacityLevelText;
        
        [Header("Reward Ad Settings")]
        [SerializeField] private Button _rewardAdButton;
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private int _reward;
        
        private CashValue _cashValue;

        protected override void OnAwake()
        {
            base.OnAwake();
            _cashValue = new CashValue { Value = _reward };
            _rewardAdButton.onClick.AddListener(OnRewardAdClicked);
        }

        private void OnDestroy() => 
            _rewardAdButton.onClick.RemoveListener(OnRewardAdClicked);

        protected override void Initialize()
        {
            UpdateRewardText();
        }

        protected override void SubscribeUpdates()
        {
        }

        protected override void CleanUp()
        {
        }

        private void OnRewardAdClicked()
        {
            //Reward Logic
            Progress.WorldData.CashData.Collect(_cashValue);
        }

        private void UpdateRewardText() => 
            _rewardText.text = $"+{_reward}$";
    }
}