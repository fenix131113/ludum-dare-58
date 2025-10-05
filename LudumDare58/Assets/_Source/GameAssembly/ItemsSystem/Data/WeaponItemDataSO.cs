using HealthSystem;
using UnityEngine;

namespace ItemsSystem.Data
{
    [CreateAssetMenu(fileName = "new WeaponItemDataSO", menuName = "SO/Items/WeaponItemDataSO")]
    public class WeaponItemDataSO : ItemDataSO
    {
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float AttackDistance { get; private set; }
        [field: SerializeField] public float ReloadTime { get; private set; }
        [field: SerializeField] public float ShootIntervalTime { get; private set; }
        [field: SerializeField] public HealthType[] DamageTo { get; private set; }
    }
}