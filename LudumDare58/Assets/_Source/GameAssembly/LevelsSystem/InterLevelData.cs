using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BaseSystem;
using InventorySystem;
using ItemsSystem;
using PlayerSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using VContainer;

namespace LevelsSystem
{
    public class InterLevelData : MonoBehaviour
    {
        public IReadOnlyList<int> CompletedLevels => _completedLevels;
        
        [Inject] private Inventory _playerInventory;
        [Inject] private PlayerResources _playerResources;

        private List<Item> _playerItems = new();
        private readonly List<int> _completedLevels = new();
        private readonly HashSet<CollectableMonsterType> _collectedMonsters = new();
        private LevelTransition _transition;

        private void Start()
        {
            // Start works only on start lobby scene and will delete any new empty InterLevelData
            if (FindObjectsByType<InterLevelData>(FindObjectsInactive.Include, FindObjectsSortMode.None).Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void LoadData()
        {
            _playerItems.ForEach(x => _playerInventory.TryAddItem(x));
            _playerResources.SetCollectedMonster(_collectedMonsters);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            StartCoroutine(DelayedStart());
        }

        private void OnSceneTransition()
        {
            if (!_completedLevels.Contains(SceneManager.GetActiveScene().buildIndex))
                _completedLevels.Add(SceneManager.GetActiveScene().buildIndex);
            
            _playerItems = _playerInventory.Items.ToList();
            Expose();
        }

        private void Bind()
        {
            _transition.OnTransition += OnSceneTransition;
        }

        private void Expose()
        {
            _transition.OnTransition -= OnSceneTransition;
        }

        private IEnumerator DelayedStart()
        {
            yield return null;

            _transition = FindFirstObjectByType<LevelTransition>();
            ObjectInjector.InjectGameObject(gameObject);
            Bind();
            LoadData();
        }
    }
}