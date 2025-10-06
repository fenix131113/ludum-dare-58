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
        [Inject] private Inventory _playerInventory;
        [Inject] private PlayerResources _playerResources;
        
        private List<Item> _playerItems = new();
        private readonly HashSet<CollectableMonsterType> _collectedMonsters = new();
        private LevelTransition _transition;
        
        private void Start()
        {
            if (FindAnyObjectByType<InterLevelData>())
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