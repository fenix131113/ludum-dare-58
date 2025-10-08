using System;
using UnityEngine;

namespace InteractionSystem
{
    public class BaseInteractiveObject : MonoBehaviour, IInteractable
    {
        public event Action OnInteracted;
        
        public void Interact()
        {
            OnInteracted?.Invoke();
        }
    }
}