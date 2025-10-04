using System.Linq;
using Core;
using Core.Data;
using InteractionSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

// ReSharper disable Unity.PreferNonAllocApi

namespace PlayerSystem
{
    public class PlayerInteract : MonoBehaviour
    {
        [SerializeField] private Transform interactPoint;
        [SerializeField] private float interactRadius;

        [Inject] private InputSystem_Actions _input;
        [Inject] private GameVariables _gameVariables;

        private GameObject _currentTarget;
        private AInteractView _currentTargetView;

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void FixedUpdate() => CheckForInteractions();

        private void CheckForInteractions()
        {
            var overlapped = Physics2D.OverlapCircleAll(interactPoint.position, interactRadius,
                LayersDataSO.Instance.InteractableLayer);

            if (_currentTarget && !overlapped.Select(x => x.gameObject).Contains(_currentTarget))
            {
                _currentTargetView?.OnInteractDisabled();
                _currentTarget = null;
                _currentTargetView = null;
            }
            
            if (overlapped.Length == 0)
                return;

            var cast = Physics2D.Raycast(transform.position, overlapped[0].transform.position - transform.position,
                ~LayersDataSO.Instance.PlayerLayer);

            if (cast.transform.GetComponent<IInteractable>() == null)
                return;
            
            _currentTarget = cast.transform.gameObject;
            
            if (!_currentTarget.TryGetComponent(out AInteractView view))
                return;
            
            _currentTargetView = view;
            view.OnInteractEnabled();
        }

        private void Interact(InputAction.CallbackContext callbackContext)
        {
            if (!_currentTarget || _gameVariables.CanInteract)
                return;

            _currentTargetView?.OnInteract();
            _currentTarget.GetComponent<IInteractable>().Interact();
        }

        private void Bind() => _input.Player.Interact.performed += Interact;

        private void Expose() => _input.Player.Interact.performed -= Interact;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!interactPoint || !Selection.activeGameObject)
                return;

            if (Selection.activeGameObject != gameObject && !Selection.activeGameObject.transform.IsChildOf(transform))
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(interactPoint.position, interactRadius);
        }
#endif
    }
}