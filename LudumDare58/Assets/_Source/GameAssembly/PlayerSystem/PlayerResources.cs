using System;
using System.Collections.Generic;
using BaseSystem;

namespace PlayerSystem
{
    public class PlayerResources
    {
        public int JarsCount { get; private set; }
        public int Coins { get; private set; } = 5000;

        public List<CollectableMonsterType> CollectedMonsters { get; private set; } = new();
        
        public event Action OnCoinsChanged;
        public event Action OnJarsCountChanged;

        public void IncreaseJarsCount()
        {
            JarsCount++;
            OnJarsCountChanged?.Invoke();
        }

        public bool TryChangeCoins(int coins)
        {
            if (Coins + coins < 0)
                return false;

            Coins += coins;
         
            OnCoinsChanged?.Invoke();
            return true;
        }

        public void AddCollectedMonster(CollectableMonsterType monster) => CollectedMonsters.Add(monster);
        public void SetCollectedMonster(List<CollectableMonsterType> set) => CollectedMonsters = set;
    }
}