using System;
using UnityEngine;

namespace LevelsSystem
{
    public abstract class ALevelTransition : MonoBehaviour
    {
        [SerializeField] protected int sceneIndexToLoad;
        
        public abstract event Action OnTransition;

        public abstract void Transition();
        public abstract void SetSceneIndexToLoad(int index);
    }
}