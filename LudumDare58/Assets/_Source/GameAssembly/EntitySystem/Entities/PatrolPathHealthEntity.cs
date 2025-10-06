using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EntitySystem.Entities
{
    public class PatrolPathHealthEntity : PathHealthEntity
    {
        [SerializeField] protected Transform[] patrolPoints;
        [SerializeField] protected bool pickPointsRandomly;
        [SerializeField] protected float waitOnPointTimeMin;
        [SerializeField] protected float waitOnPointTimeMax;
        [SerializeField] protected bool patrolFromStart = true;

        public bool IsPatrol { get; protected set; }

        private int _lastPatrolPointIndex = -1;
        private Action _onReachedPatrolPoint;
        private Coroutine _waitForNextPatrolPointRoutine;

        protected override void Start()
        {
            base.Start();

            if (patrolFromStart)
                StartPatrol();
        }

        protected override void OnNativeDestinationReached()
        {
            base.OnNativeDestinationReached();

            if (IsPatrol && !destinationSetter.target)
            {
                _onReachedPatrolPoint?.Invoke();
                _onReachedPatrolPoint = null;
                if (_waitForNextPatrolPointRoutine == null)
                    _waitForNextPatrolPointRoutine = StartCoroutine(WaitUntilGoNextPatrolPoint());
            }
        }

        public override void SetTarget(Transform target)
        {
            base.SetTarget(target);

            IsPatrol = false;

            if (_waitForNextPatrolPointRoutine != null)
                StopCoroutine(_waitForNextPatrolPointRoutine);
        }

        public void StartPatrol(Action patrolPointReachedCallback = null)
        {
            if (patrolPoints.Length == 0)
                return;

            if (patrolPointReachedCallback != null)
                _onReachedPatrolPoint += patrolPointReachedCallback;

            if (pickPointsRandomly)
            {
                var toExcept = new Transform[1];

                if (_lastPatrolPointIndex != -1)
                    toExcept[0] = patrolPoints[_lastPatrolPointIndex];

                var exceptArray = patrolPoints.Except(toExcept).ToArray();
                _lastPatrolPointIndex = Array.IndexOf(exceptArray, exceptArray[Random.Range(0, exceptArray.Length - 1)]);
            }
            else
            {
                if (_lastPatrolPointIndex == -1)
                    _lastPatrolPointIndex = 0;
                else
                    _lastPatrolPointIndex =
                        _lastPatrolPointIndex + 1 >= patrolPoints.Length ? 0 : _lastPatrolPointIndex + 1;
            }

            destinationSetter.target = patrolPoints[_lastPatrolPointIndex];
            _isNativeDestinationReached = false;
            IsPatrol = true;
        }

        public void MoveToFarthestPoint()
        {
            ResumeMoving();
            SetTarget(GetFarthestPoint());
        }

        public Transform GetFarthestPoint()
        {
            if (patrolPoints.Length == 0)
                return null;

            Transform farthest = null;

            foreach (var patrolPoint in patrolPoints)
            {
                if (!farthest)
                {
                    farthest = patrolPoint;
                    continue;
                }

                if (Vector2.Distance(transform.position, farthest.position) <
                    Vector2.Distance(transform.position, patrolPoint.position))
                    farthest = patrolPoint;
            }

            return farthest;
        }

        private IEnumerator WaitUntilGoNextPatrolPoint()
        {
            yield return new WaitForSeconds(Random.Range(waitOnPointTimeMin, waitOnPointTimeMax));

            _waitForNextPatrolPointRoutine = null;
            StartPatrol();
        }
    }
}