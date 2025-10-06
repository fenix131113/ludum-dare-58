using Core.Data;
using LevelsSystem;
using UnityEngine;
using Utils;
using VContainer;

namespace BaseSystem
{
    public class ElevatorZone : MonoBehaviour
    {
        [Inject] private LayersDataSO _layersDataSO;
        private LevelTransition _transition;
        
        private void Start() => _transition = FindFirstObjectByType<LevelTransition>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, _layersDataSO.PlayerLayer))
                return;
            
            _transition.Transition();
        }
    }
}