using EntitySystem.Entities;
using PlayerSystem;
using UnityEngine;
using VContainer;

namespace LevelsSystem
{
    public class JarsLevelTransition : MonoBehaviour
    {
        [Inject] private PlayerResources _playerResources;
        
        private ALevelTransition _levelTransition;
        private int _needJarsCount;

        private void Awake()
        {
            _needJarsCount = FindObjectsByType<PathHealthEntity>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Length;
            _levelTransition = FindFirstObjectByType<ALevelTransition>();
        }

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void OnJarsCountChanged()
        {
            if(_needJarsCount == _playerResources.JarsCount)
                _levelTransition.Transition();
        }

        private void Bind() => _playerResources.OnJarsCountChanged += OnJarsCountChanged;

        private void Expose() => _playerResources.OnJarsCountChanged -= OnJarsCountChanged;
    }
}