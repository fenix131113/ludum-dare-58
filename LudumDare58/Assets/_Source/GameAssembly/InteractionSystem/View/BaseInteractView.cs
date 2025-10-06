using UnityEngine;

namespace InteractionSystem.View
{
    public class BaseInteractView : AInteractView
    {
        [SerializeField] private GameObject interactHelper;
        
        public override void OnInteract()
        {
            
        }

        public override void OnInteractEnabled()
        {
            interactHelper.SetActive(true);
        }

        public override void OnInteractDisabled()
        {
            interactHelper.SetActive(false);
        }
    }
}