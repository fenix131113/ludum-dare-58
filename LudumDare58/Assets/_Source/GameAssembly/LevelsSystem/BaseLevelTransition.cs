using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LevelsSystem
{
    public class BaseLevelTransition : ALevelTransition
    {
        [field: SerializeField] public bool RegisterLevelAsCompleted { get; private set; }

        [SerializeField] private float fadeTime = 0.5f;
        [SerializeField] private Image screenFader;

        public override event Action OnTransition;
        
        public override void SetSceneIndexToLoad(int sceneIndex)
        {
            sceneIndexToLoad = sceneIndex;
        }

        public override void Transition()
        {
            OnTransition?.Invoke();
            screenFader.DOFade(1f, fadeTime).onComplete += OnTransitionEnded;
        }

        private void OnTransitionEnded() => SceneManager.LoadScene(sceneIndexToLoad);
    }
}