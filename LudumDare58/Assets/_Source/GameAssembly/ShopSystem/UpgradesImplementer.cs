using ItemsSystem.Data;
using UnityEngine;
using WeaponsSystem;

namespace ShopSystem
{
    public class UpgradesImplementer : MonoBehaviour
    {
        [SerializeField] private VacuumCleaner vacuumCleaner;
        [SerializeField] private CameraWeapon cameraWeapon;
        [SerializeField] private Flute flute;
        [SerializeField] private int vacuumCleanerAdditionalDamage;
        [SerializeField] private float cameraTimeReduce;
        [SerializeField] private int fluteKeysReduceCount;

        public void ImplementUpgrade(ItemDataSO data)
        {
            if (data == vacuumCleaner.ItemData)
                SetVacuumCleanerUpgrade();
            else if (data == cameraWeapon.ItemData)
                SetCameraUpgrade();
            else if (data == flute.ItemData)
                SetFluteUpgrade();
            else
                throw new System.ArgumentException($"Invalid item data: {data.name}");
        }

        private void SetVacuumCleanerUpgrade() => vacuumCleaner.SetAdditionalDamage(vacuumCleanerAdditionalDamage);

        private void SetCameraUpgrade() => cameraWeapon.SetCameraReloadReduce(cameraTimeReduce);

        private void SetFluteUpgrade() => flute.SetKeysCount(fluteKeysReduceCount);
    }
}