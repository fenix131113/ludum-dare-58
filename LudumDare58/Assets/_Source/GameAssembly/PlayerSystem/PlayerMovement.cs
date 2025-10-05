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
        private static readonly int _x = Animator.StringToHash("X");
        private static readonly int _y = Animator.StringToHash("Y");
        
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Animator anim;

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

        public void Move(Vector2 movement)
        {
            anim.SetFloat(_x, movement.x);
            anim.SetFloat(_y, movement.y);
            rb.linearVelocity = movement * (_playerConfig.Speed * Time.fixedDeltaTime);
        }

        public Entity GetEntity() => _entity;
    }
}