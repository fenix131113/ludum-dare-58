using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(fileName = "New LayersData", menuName = "LayersData")]
    public class LayersDataSO : ScriptableObject
    {
        [field: SerializeField] public LayerMask PlayerLayer { get; private set; }
        [field: SerializeField] public LayerMask InteractableLayer { get; private set; }
        
        public static LayersDataSO Instance { get; private set; }

        public static void SetupLayersInstance(LayersDataSO instance)
        {
            if(instance)
               return;
            
            Instance = instance;
        }
    }
}