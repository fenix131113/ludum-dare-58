using HealthSystem;
using UnityEngine;

namespace ItemsSystem.Data
{
    [CreateAssetMenu(fileName = "new WeaponItemDataSO", menuName = "SO/Items/WeaponItemDataSO")]
    public class WeaponItemDataSO : ItemDataSO // TODO: Remove and place extra data in weapons classes, i was wrong .·´¯`(>▂<)´¯`·. 
    {
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float AttackDistance { get; private set; }
        [field: SerializeField] public float ReloadTime { get; private set; }
        [field: SerializeField] public float ShootIntervalTime { get; private set; }
        [field: SerializeField] public HealthType[] DamageTo { get; private set; }
    }
}