using System;
using InteractionSystem;
using PlayerSystem;
using UnityEngine;
using VContainer;

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