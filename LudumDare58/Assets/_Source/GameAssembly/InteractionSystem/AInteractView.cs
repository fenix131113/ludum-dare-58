using UnityEngine;

namespace InteractionSystem
{
    public abstract class AInteractView : MonoBehaviour
    {
        public virtual void OnInteract(){}
        public virtual void OnInteractEnabled(){}
        public virtual void OnInteractDisabled(){}
    }
}