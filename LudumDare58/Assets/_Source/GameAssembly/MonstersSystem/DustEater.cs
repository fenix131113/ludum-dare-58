using EntitySystem.Entities;
using EntitySystem.Entities.Interfaces;
using PlayerSystem;
using UnityEngine;

namespace MonstersSystem
{
    public class DustEater : MonoBehaviour, IEntityContains
    {
        private PathHealthEntity _entity;

        private void Awake() => _entity = GetComponent<PathHealthEntity>();

        private void Start()
        {
            _entity.SetTarget(FindFirstObjectByType<PlayerMovement>().transform);
        }

        public Entity GetEntity() => _entity;
    }
}