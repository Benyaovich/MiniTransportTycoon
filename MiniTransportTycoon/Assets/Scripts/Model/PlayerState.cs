#nullable enable
using System;

namespace Model
{
    public class PlayerState
    {
        private static PlayerState? _instance;

        public static PlayerState Instance
        {
            get
            {
                _instance ??= new PlayerState();
                return _instance;
            }
        }

        public int Money { get; private set; } = 10000;
        public bool IsGameOver { get; private set; }

        public event EventHandler<int>? OnMoneyChanged;
        public event EventHandler? OnGameOver;

        public void ResetPlayerState()
        {
            Money = 10000;
            IsGameOver = false;
        }
        
        public void AddMoney(int amount)
        {
            Money += amount;
            OnMoneyChanged?.Invoke(EventArgs.Empty, Money);
        }

        public void SetMoney(int amount)
        {
            Money = amount;
            OnMoneyChanged?.Invoke(EventArgs.Empty, Money);
        }

        public void SpendMoney(int amount)
        {
            Money -= amount;

            if (Money <= 0)
            {
                IsGameOver = true;
                OnGameOver?.Invoke(this,EventArgs.Empty);
            }

            OnMoneyChanged?.Invoke(EventArgs.Empty, Money);
        }
    }
}
