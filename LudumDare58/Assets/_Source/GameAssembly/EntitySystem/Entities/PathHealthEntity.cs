using Pathfinding;
using UnityEngine;

namespace EntitySystem.Entities
{
    public class PathHealthEntity : HealthEntity
    {
        [SerializeField] protected AIPath path;
        [SerializeField] protected AIDestinationSetter destinationSetter;

        public void SetTarget(Transform target) => destinationSetter.target = target;

        public void StopMoving() => path.isStopped = true;

        public void ResumeMoving()
        {
            if (destinationSetter.target)
                path.isStopped = false;
        }
    }
}