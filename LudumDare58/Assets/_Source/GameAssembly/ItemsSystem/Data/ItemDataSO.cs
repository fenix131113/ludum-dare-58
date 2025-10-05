using UnityEngine;

namespace ItemsSystem.Data
{
    [CreateAssetMenu(fileName = "new ItemDataSO", menuName = "SO/Items/ItemDataSO")]
    public class ItemDataSO : ScriptableObject
    {
        [field: SerializeField] public string ItemName { get; private set; }
        [field: SerializeField] public int MaxCount { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

        public Item GenerateItemInstance(int count = 1) => new(this, count);
    }
}