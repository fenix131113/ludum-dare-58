using EntitySystem.Entities;
using PlayerSystem;
using TMPro;
using UnityEngine;
using VContainer;

namespace LevelsSystem
{
    public class JarsLevelTransition : MonoBehaviour
    {
        [Inject] private PlayerResources _playerResources;

        [SerializeField] private TMP_Text counterText;

        private ALevelTransition _levelTransition;
        private int _needJarsCount;

        private void Awake()
        {
            _needJarsCount = FindObjectsByType<PathHealthEntity>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Length;
            _levelTransition = FindFirstObjectByType<ALevelTransition>();

            counterText.text = $"{_needJarsCount}";
        }

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void OnJarsCountChanged()
        {
            counterText.text = $"{_needJarsCount - _playerResources.JarsCount}";
            if (_needJarsCount == _playerResources.JarsCount)
                _levelTransition.Transition();
        }

        private void Bind() => _playerResources.OnJarsCountChanged += OnJarsCountChanged;

        private void Expose() => _playerResources.OnJarsCountChanged -= OnJarsCountChanged;
    }
}