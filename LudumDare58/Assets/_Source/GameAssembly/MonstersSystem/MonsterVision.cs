using System;
using System.Collections;
using UnityEngine;
using Utils;

namespace MonstersSystem
{
    public class MonsterVision : MonoBehaviour
    {
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private LayerMask ignoreLayer;
        [SerializeField] private bool checkWithRaycast;
        [SerializeField] private float recheckRayInterval;

        public Transform Target { get; private set; }
        public bool CanSeeTarget => Target;

        private Transform _targetInZone;

        public event Action OnTargetSpotted;
        public event Action OnTargetLost;

        private void LostTarget()
        {
            StopAllCoroutines();
            Target = null;
            OnTargetLost?.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, targetLayer))
                return;

            _targetInZone = other.transform;

            if (checkWithRaycast)
            {
                StartCoroutine(RecheckPlayerWithRayCoroutine());

                var rayResult = Physics2D.Raycast(transform.position, other.transform.position - transform.position,
                    float.PositiveInfinity, ~ignoreLayer);

                if (!rayResult || !LayerService.CheckLayersEquality(rayResult.transform.gameObject.layer, targetLayer))
                    return;
            }

            Target = other.transform;
            OnTargetSpotted?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.transform != _targetInZone)
                return;
            
            _targetInZone = null;
            LostTarget();
        }

        // ReSharper disable once FunctionRecursiveOnAllPaths
        private IEnumerator RecheckPlayerWithRayCoroutine()
        {
            yield return new WaitForSeconds(recheckRayInterval);

            StartCoroutine(RecheckPlayerWithRayCoroutine());
            
            if (!CanSeeTarget)
            {
                var rayResult = Physics2D.Raycast(transform.position, _targetInZone.position - transform.position,
                    float.PositiveInfinity, ~ignoreLayer);

                if (!rayResult || !LayerService.CheckLayersEquality(rayResult.transform.gameObject.layer, targetLayer))
                {
                    StartCoroutine(RecheckPlayerWithRayCoroutine());
                    yield break;
                }

                StartCoroutine(RecheckPlayerWithRayCoroutine());
                Target = rayResult.transform;
                OnTargetSpotted?.Invoke();
            }
            else
            {
                var rayResult = Physics2D.Raycast(transform.position, Target.position - transform.position,
                    float.PositiveInfinity, ~ignoreLayer);

                if (!rayResult || rayResult.transform != Target)
                    LostTarget();
            }
        }
    }
}