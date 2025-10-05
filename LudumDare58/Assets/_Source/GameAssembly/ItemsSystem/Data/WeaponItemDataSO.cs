using UnityEngine;

namespace ItemsSystem.Data
{
    [CreateAssetMenu(fileName = "new WeaponItemDataSO", menuName = "SO/Items/WeaponItemDataSO")]
    public class WeaponItemDataSO : ItemDataSO
    {
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float Radius { get; private set; }
        [field: SerializeField] public float ReloadTime { get; private set; }
        [field: SerializeField] public float ShootIntervalTime { get; private set; }
    }
}