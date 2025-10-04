using UnityEngine;

namespace EntitySystem.Data
{
    [CreateAssetMenu(fileName = "new EntityConfig", menuName = "SO/EntityConfig")]
    public class EntityConfigSO : ScriptableObject
    {
        [field: SerializeField] public bool TriggerCollision { get; private set; }
        [field: SerializeField] public bool PhysicEntity { get; private set; }
    }
}