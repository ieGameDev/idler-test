using System;

namespace Game.Scripts.Data
{
    [Serializable]
    public class UpgradesData
    {
        public event Action OnChanged;
        
        public int Capacity;
        public int PlayerLevel;
        
        public void UpgradeCapacity(int value)
        {
            Capacity = value;
            OnChanged?.Invoke();
        }

        public void UpgradePlayerLevel()
        {
            PlayerLevel++;
            OnChanged?.Invoke();
        }
    }
}