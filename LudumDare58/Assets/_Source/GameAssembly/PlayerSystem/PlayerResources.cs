using System;
using System.Collections.Generic;
using BaseSystem;

namespace PlayerSystem
{
    public class PlayerResources
    {
        public int JarsCount { get; private set; }

        public HashSet<CollectableMonsterType> CollectedMonsters { get; private set; } = new();

        public void IncreaseJarsCount()
        {
            JarsCount++;
            OnJarsCountChanged?.Invoke();
        }

        public void AddCollectedMonster(CollectableMonsterType monster) => CollectedMonsters.Add(monster);

        public void SetCollectedMonster(HashSet<CollectableMonsterType> set) => CollectedMonsters = set;

        public event Action OnJarsCountChanged;
    }
}