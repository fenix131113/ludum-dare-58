using System.Collections.Generic;
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
        [SerializeField] private ParticleSystem walkParticles;
        [SerializeField] private float emitParticleInterval;
        [SerializeField] private Animator anim;
        [SerializeField] private List<AudioClip> walkSounds;
        [SerializeField] private float walkSoundInterval;
        [SerializeField] private AudioSource walkSource;

        [Inject] private InputSystem_Actions _input;
        [Inject] private PlayerConfigSO _playerConfig;
        [Inject] private GameVariables _gameVariables;
        private Entity _entity;
        private float _lastEmitParticleTime;
        private float _walkSoundTimer;

        private void Awake()
        {
            _entity = GetComponent<Entity>();
            _walkSoundTimer = walkSoundInterval;
        }

        private void FixedUpdate()
        {
            if (_gameVariables.CanMove && _input.Player.enabled)
            {
                Move(_input.Player.Move.ReadValue<Vector2>()); // Normalized in InputActions
            }
            else
                Move(Vector2.zero);
        }

        public void Move(Vector2 movement)
        {
            if (movement.magnitude != 0 && Time.time >= _lastEmitParticleTime + emitParticleInterval && walkParticles &&
                walkParticles.IsAlive(true))
            {
                walkParticles.Emit(Random.Range(1, 4));
                _lastEmitParticleTime = Time.time;
                
                _walkSoundTimer -= Time.fixedDeltaTime;
                if (_walkSoundTimer <= 0)
                {
                    _walkSoundTimer = walkSoundInterval;
                    walkSource.PlayOneShot(walkSounds[Random.Range(0, walkSounds.Count)]);
                }
            }

            anim.SetFloat(_x, movement.x);
            anim.SetFloat(_y, movement.y);
            rb.linearVelocity = movement * (_playerConfig.Speed * Time.fixedDeltaTime);
        }

        public Entity GetEntity() => _entity;
    }
}