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
        private ALevelTransition _levelTransition;
        
        private void Start() => _levelTransition = FindFirstObjectByType<ALevelTransition>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, _layersDataSO.PlayerLayer))
                return;
            
            _levelTransition.Transition();
        }
    }
}