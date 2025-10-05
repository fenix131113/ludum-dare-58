using Core;
using EntitySystem.Entities;
using EntitySystem.Entities.Interfaces;
using PlayerSystem.Data;
using UnityEngine;
using VContainer;

namespace PlayerSystem
{
    [RequireComponent(typeof(Entity))]
    public class PlayerMovement : MonoBehaviour, IMoveEntity
    {
        [SerializeField] private Rigidbody2D rb;

        [Inject] private InputSystem_Actions _input;
        [Inject] private PlayerConfigSO _playerConfig;
        [Inject] private GameVariables _gameVariables;
        private Entity _entity;

        private void Awake() => _entity = GetComponent<Entity>();

        private void FixedUpdate()
        {
            if (_gameVariables.CanMove)
                Move(_input.Player.Move.ReadValue<Vector2>()); // Normalized in InputActions
        }

        public void Move(Vector2 movement) =>
            rb.linearVelocity = movement * (_playerConfig.Speed * Time.fixedDeltaTime);

        public Entity GetEntity() => _entity;
    }
}