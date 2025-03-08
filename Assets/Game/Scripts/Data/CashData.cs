using System;

namespace Game.Scripts.Data
{
    [Serializable]
    public class CashData
    {
        public int Collected;
        public event Action OnChanged;

        public void Collect(CashValue cash)
        {
            Collected += cash.Value;
            OnChanged?.Invoke();
        }

        public void Spend(int price)
        {
            Collected -= price;
            OnChanged?.Invoke();
        }
    }
}