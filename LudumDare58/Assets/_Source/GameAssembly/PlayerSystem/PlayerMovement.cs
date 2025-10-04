using EntitySystem;
using PlayerSystem.Data;
using UnityEngine;
using VContainer;

namespace PlayerSystem
{
    [RequireComponent(typeof(Entity))]
    public class PlayerMovement : MonoBehaviour, IMoveEntity
    {
        [SerializeField] private Entity entity;
        [SerializeField] private Rigidbody2D rb;

        [Inject] private InputSystem_Actions _input;
        [Inject] private PlayerConfigSO _playerConfig;

        private void FixedUpdate() => Move(_input.Player.Move.ReadValue<Vector2>());

        public void Move(Vector2 movement) =>
            rb.linearVelocity = movement * (_playerConfig.Speed * Time.fixedDeltaTime);
    }
}