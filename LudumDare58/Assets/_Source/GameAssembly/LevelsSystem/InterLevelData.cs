using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InventorySystem;
using ItemsSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using VContainer;

namespace LevelsSystem
{
    public class InterLevelData : MonoBehaviour
    {
        [Inject] private Inventory _playerInventory;
        
        private List<Item> _playerItems = new();
        private LevelTransition _transition;
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void LoadData()
        {
            _playerItems.ForEach(x => _playerInventory.TryAddItem(x));
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