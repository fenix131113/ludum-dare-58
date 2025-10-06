using System;
using Pathfinding;
using UnityEngine;

namespace EntitySystem.Entities
{
    public class PathHealthEntity : HealthEntity
    {
        [SerializeField] protected AIPath path;
        [SerializeField] protected AIDestinationSetter destinationSetter;

        protected bool _isNativeDestinationReached;

        public event Action OnNativePathReached;

        protected virtual void Update()
        {
            if (!_isNativeDestinationReached && path.reachedEndOfPath)
            {
                var distance = Vector3.Distance(transform.position, destinationSetter.target.position);
                
                if (path.reachedEndOfPath && distance <= path.endReachedDistance)
                    OnNativeDestinationReached();
            }
        }

        protected virtual void OnNativeDestinationReached()
        {
            destinationSetter.target = null;
            _isNativeDestinationReached = true;
            OnNativePathReached?.Invoke();
        }

        public virtual void SetTarget(Transform target)
        {
            destinationSetter.target = target;
            _isNativeDestinationReached = false;
        }

        public virtual void StopMoving() => path.isStopped = true;

        public virtual void ResumeMoving() => path.isStopped = false;
    }
}