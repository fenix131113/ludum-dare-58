using Core;
using Core.Data;
using EntitySystem.Entities;
using EntitySystem.Entities.Interfaces;
using PlayerSystem.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
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
        [SerializeField] private Collider2D col;

        [Header("Dash Settings")]
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashDistance;
        [SerializeField] private float dashCooldown;
        [SerializeField] private Image filler;
        [SerializeField] private GameObject barObject;

        [Inject] private InputSystem_Actions _input;
        [Inject] private PlayerConfigSO _playerConfig;
        [Inject] private GameVariables _gameVariables;
        [Inject] private LayersDataSO _layersData;

        private Entity _entity;
        private float _lastEmitParticleTime;
        private float _walkSoundTimer;

        private Vector2 _lastMovementDirection;

        private float _dashTimer;
        private bool _isDashing;

        private void Awake()
        {
            _entity = GetComponent<Entity>();
            _walkSoundTimer = walkSoundInterval;
        }

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void FixedUpdate()
        {
            if (_input.Player.Move.ReadValue<Vector2>() != Vector2.zero)
                _lastMovementDirection = _input.Player.Move.ReadValue<Vector2>();
            if (_isDashing) return;
            if (_gameVariables.CanMove && _input.Player.enabled)
            {
                Move(_input.Player.Move.ReadValue<Vector2>()); // Normalized in InputActions
            }
            else
                Move(Vector2.zero);
        }

        private void Update()
        {
            filler.fillAmount = _dashTimer - Time.time;
            if (Time.time > _dashTimer) barObject.SetActive(false);
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

        private void Dash(InputAction.CallbackContext callbackContext)
        {
            if (!_gameVariables.CanMove || !_input.Player.enabled || !(Time.time > _dashTimer))
                return;
            StartCoroutine(Dashing());
            _dashTimer = Time.time + dashCooldown;
            barObject.SetActive(true);
        }

        private IEnumerator Dashing()
        {
            var moveDirection = _lastMovementDirection;
            var startPosition = new Vector2 (col.transform.position.x,col.transform.position.y);
            var neededPosition = startPosition + (moveDirection * dashDistance);
            var hit = Physics2D.CircleCast(startPosition,0.8f, moveDirection,dashDistance, _layersData.ObstacleLayer);
            _isDashing = true;
            rb.linearVelocity = moveDirection * ((dashSpeed + _playerConfig.Speed) * Time.fixedDeltaTime);
            if (hit && hit.distance < Vector2.Distance(startPosition, neededPosition))
            {
                yield return new WaitUntil(() => Physics2D.IsTouching(col, hit.collider)|| Vector2.Distance(startPosition, rb.position) >= Vector2.Distance(startPosition, neededPosition));
            }
            else
            {
                yield return new WaitUntil(() => Vector2.Distance(startPosition, rb.position) >= Vector2.Distance(startPosition, neededPosition));
            }

            rb.linearVelocity *= 0;
            _isDashing = false;
        }

        private void Bind() => _input.Player.Dash.performed += Dash;

        private void Expose() => _input.Player.Dash.performed -= Dash;

        public Entity GetEntity() => _entity;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(new Vector2(col.transform.position.x, col.transform.position.y), _lastMovementDirection);
        }
#endif
    }
}