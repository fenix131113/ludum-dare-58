using System;

namespace InteractionSystem
{
    public interface IInteractable
    {
        event Action OnInteracted;
        
        void Interact();
    }
}