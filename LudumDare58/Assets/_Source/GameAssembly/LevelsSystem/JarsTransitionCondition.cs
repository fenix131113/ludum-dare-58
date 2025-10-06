using MonstersSystem;
using UnityEngine;

namespace LevelsSystem
{
    public class JarsTransitionCondition : MonoBehaviour
    {
        private int _needJarsCount;

        private void Awake()
        {
            _needJarsCount = FindObjectsByType<PatrolMonster>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Length;
        }
    }
}