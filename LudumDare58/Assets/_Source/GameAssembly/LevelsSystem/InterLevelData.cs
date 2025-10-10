using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BaseSystem;
using InventorySystem;
using ItemsSystem;
using ItemsSystem.Data;
using PlayerSystem;
using ShopSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using VContainer;

namespace LevelsSystem
{
    public class InterLevelData : MonoBehaviour
    {
        public IReadOnlyList<int> CompletedLevels => _completedLevels;
        public IReadOnlyList<CollectableMonsterType> CollectedMonsters => _collectedMonsters;
        public IReadOnlyDictionary<ItemDataSO, int> BoughtItems => _boughtItems;
        public int MoneyToGet { get; private set; } = 50;

        [Inject] private Inventory _playerInventory;
        [Inject] private PlayerResources _playerResources;
        [Inject] private UpgradesImplementer _upgradesImplementer;

        private Dictionary<ItemDataSO, int> _boughtItems = new();
        private List<Item> _playerItems = new();
        private readonly List<int> _completedLevels = new();
        private readonly List<CollectableMonsterType> _collectedMonsters = new();
        private ALevelTransition _levelTransition;
        private int _lastGameSceneIndex;

        private void Start()
        {
            _levelTransition = FindFirstObjectByType<ALevelTransition>();
            InitObject();

            if (SceneManager.GetActiveScene().buildIndex == 0 &&
                _lastGameSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
            {
                _lastGameSceneIndex++;
                _levelTransition.SetSceneIndexToLoad(_lastGameSceneIndex);
            }

            // Start works only on start lobby scene and will delete any new empty InterLevelData
            if (FindObjectsByType<InterLevelData>(FindObjectsInactive.Include, FindObjectsSortMode.None).Length > 1)
            {
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void LoadDelayedData()
        {
            _playerItems.ForEach(x => _playerInventory.TryAddItem(x));
            _playerResources.SetCollectedMonster(_collectedMonsters);
            
            foreach (var item in BoughtItems)
                _upgradesImplementer.ImplementUpgrade(item.Key);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            StartCoroutine(DelayedStart());
        }

        private void OnSceneLevelTransition()
        {
            if (SceneManager.GetActiveScene().buildIndex != 0 &&
                _lastGameSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
            {
                _lastGameSceneIndex++;
            }

            if (!_completedLevels.Contains(SceneManager.GetActiveScene().buildIndex))
                _completedLevels.Add(SceneManager.GetActiveScene().buildIndex);
            
            _playerItems = _playerInventory.Items.ToList();
            Expose();
        }
        
        private void InitObject()
        {
            ObjectInjector.InjectGameObject(gameObject);
            Bind();
        }

        public void SetBoughtItems(Dictionary<ItemDataSO, int> boughtItems) => _boughtItems = boughtItems;

        public void IncreaseMoneyToGet(int amount) => MoneyToGet += amount;

        public void ClearGetMoney() => MoneyToGet = 0;

        private void Bind()
        {
            _levelTransition.OnTransition += OnSceneLevelTransition;
        }

        private void Expose()
        {
            _levelTransition.OnTransition -= OnSceneLevelTransition;
        }

        private IEnumerator DelayedStart()
        {
            yield return null;

            _levelTransition = FindFirstObjectByType<BaseLevelTransition>();
            InitObject();
            LoadDelayedData();

            if (SceneManager.GetActiveScene().buildIndex == 0)
                _levelTransition.SetSceneIndexToLoad(_lastGameSceneIndex);
        }
    }
}