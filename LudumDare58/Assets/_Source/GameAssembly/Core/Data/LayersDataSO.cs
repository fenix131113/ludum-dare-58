using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(fileName = "New LayersData", menuName = "SO/LayersData")]
    public class LayersDataSO : ScriptableObject
    {
        [field: SerializeField] public LayerMask PlayerLayer { get; private set; }
        [field: SerializeField] public LayerMask InteractableLayer { get; private set; }
    }
}