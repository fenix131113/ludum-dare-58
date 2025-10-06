using MonstersSystem;
using PlayerSystem;
using UnityEngine;
using VContainer;

namespace LevelsSystem
{
    public class JarsTransitionCondition : MonoBehaviour
    {
        [Inject] private PlayerResources _playerResources;
        
        private LevelTransition _levelTransition;
        private int _needJarsCount;

        private void Awake()
        {
            _needJarsCount = FindObjectsByType<PatrolMonster>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Length;
            _levelTransition = FindFirstObjectByType<LevelTransition>();
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