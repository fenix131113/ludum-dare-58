using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LevelsSystem
{
    public class LevelTransition : MonoBehaviour
    {
        [field: SerializeField] public bool RegisterLevelAsCompleted { get; private set; }

        [SerializeField] private float fadeTime = 0.5f;
        [SerializeField] private Image screenFader;
        [SerializeField] private int sceneIndexToLoad;
        
        public event Action OnTransition;


        public void SetSceneIndexToLoad(int sceneIndex)
        {
            sceneIndexToLoad = sceneIndex;
            Debug.Log(name);
        }

        public void Transition()
        {
            OnTransition?.Invoke();
            screenFader.DOFade(1f, fadeTime).onComplete += OnTransitionEnded;
        }

        private void OnTransitionEnded() => SceneManager.LoadScene(sceneIndexToLoad);
    }
}