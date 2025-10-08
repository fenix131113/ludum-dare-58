using System;
using InteractionSystem;
using UnityEngine;

namespace BaseSystem
{
    public class Lectern : MonoBehaviour, IInteractable
    {
        public event Action OnInteracted;
        
        public void Interact()
        {
            
            
            OnInteracted?.Invoke();
        }
    }
}