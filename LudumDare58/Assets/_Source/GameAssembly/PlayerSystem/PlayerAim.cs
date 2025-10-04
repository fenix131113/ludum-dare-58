using Core;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace PlayerSystem
{
    public class PlayerAim : MonoBehaviour
    {
        public float RotateAngle { get; private set; }
        
        [Inject] private GameVariables _gameVariables;

        private void Update()
        {
            if(!_gameVariables.CanRotate)
                return;
            
            var rotVector = Camera.main!.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
            RotateAngle = Mathf.Atan2(rotVector.y, rotVector.x) * Mathf.Rad2Deg;
        }
    }
}