using System;
using BaseSystem;
using PlayerSystem;
using UnityEngine;
using Utils;
using VContainer;

namespace InteractionSystem.Objects
{
    public class MonsterJar : MonoBehaviour, IInteractable
    {
        [SerializeField] private CollectableMonsterType monsterType;
        
        [Inject] private PlayerResources _resources;
        
        public event Action OnInteracted;

        private void Start()
        {
            ObjectInjector.InjectGameObject(gameObject);
        }

        public void Interact()
        {
            _resources.AddCollectedMonster(monsterType);
            _resources.IncreaseJarsCount();
            
            OnInteracted?.Invoke();
            
            Destroy(gameObject);
        }
    }
}