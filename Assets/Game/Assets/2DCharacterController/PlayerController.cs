using MEC;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;


namespace TeamTheDream
{
    public class PlayerController : SerializedWorldActor
    {
        [SerializeField, BoxGroup("Renderer")]
        protected Transform _rendererTransform;

        [SerializeField, BoxGroup("Renderer")]
        protected IAnimatorController _animator;

        [SerializeField, BoxGroup("Input")]
        protected IInputController _input;

        [SerializeField, BoxGroup("Physics")]
        protected Rigidbody2D _rigidBody;

        [SerializeField, BoxGroup("Physics")]
        protected BoxCollider2D _collider;

        [SerializeField, BoxGroup("Physics")]
        protected LayerMask _platformMask;

        [SerializeField, BoxGroup("Physics")]
        protected LayerMask _groundMask;

        [SerializeField, BoxGroup("Behaviour")]
        protected float _checkGroundOffset = 0.2f;

        [SerializeField, BoxGroup("Behaviour")]
        protected float _jumpForce = 5f;

        //[SerializeField, BoxGroup("Behaviour")]
        //protected float _hangingDownForce = 1f;

        [SerializeField, BoxGroup("Behaviour")]
        protected float _speed = 4f;

        [SerializeField, BoxGroup("Behaviour")]
        protected float _crouchSpeed = 1f;

        [SerializeField, BoxGroup("Behaviour")]
        protected float _climbingSpeed = 2f;

        [SerializeField, BoxGroup("Behaviour")]
        protected float _fallMultiplier = 2.5f;

        [SerializeField, BoxGroup("Behaviour")]
        protected float _lowJumpMultiplier = 2f;

        [SerializeField, BoxGroup("Behaviour")]
        protected float _crouchSizeMultiplier = 0.5f;

        [SerializeField, BoxGroup("Health")]
        protected int _baseHealth = 100;

        [SerializeField, BoxGroup("Health")]
        protected int _maxHealth = 100;

        [SerializeField, BoxGroup("Attack")]
        protected Attack[] _attacks;

        [SerializeField, BoxGroup("Attack")]
        protected Attack _airAttack;

        [SerializeField, BoxGroup("Attack")]
        protected Attack _crouchAttack;

        [SerializeField, BoxGroup("Attack")]
        protected float _chainAttackDuration = 0.5f;

        [SerializeField, BoxGroup("Damage")]
        protected float _fallDamageMinSpeed = 10f;

        [SerializeField, BoxGroup("Damage")]
        protected float _fallDamageMaxSpeed = 10f;

        [SerializeField, BoxGroup("Damage")]
        protected float _damagePerOverspeed = 10f;

        [SerializeField, BoxGroup("Damage")]
        protected float _invulnerabilityTimeAfterTakeDamage = 0.2f;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State")]
        protected Vector2 _standColliderSize;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State")]
        protected Vector2 _standColliderOffset;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State")]
        protected Vector2 _crouchColliderSize;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State")]
        protected Vector2 _crouchColliderOffset;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Actions")]
        protected bool _isRunning;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Actions")]
        protected bool _isJumping;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Actions")]
        protected bool _isFalling;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Actions")]
        protected bool _isCrouch;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Actions")]
        protected bool _isGrounded;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Actions")]
        protected bool _isClimbling;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Actions")]
        protected bool _isHanging;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Actions")]
        protected bool _isLadding;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Actions")]
        protected bool _isHangingDown;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Actions")]
        protected bool _isAttacking;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Actions")]
        protected float _chainAttackEndTime;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Actions")]
        protected float _attackComboEndTime;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Actions")]
        protected int _attackCombo = 0;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Environment")]
        protected Collider2D _currentGround;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Environment")]
        protected Platform _platform;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Environment")]
        protected Ladder _ladder;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Environment")]
        protected IInteractable _activeInteractable;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Environment")]
        protected List<IInteractable> _possibleInteractables;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Combat")]
        protected int _currentHealth;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Combat")]
        protected float _lastDamageTakenTime;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Combat")]
        protected float _stuntEndTime;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Combat")]
        protected float _lastFallSpeed;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State/Combat")]
        protected bool _isAlive;

        public int CurrentHealth {
            get => _currentHealth;
            set {
                int clampedValue = Mathf.Clamp(value, 0, _maxHealth);
                
                if (clampedValue != _currentHealth)
                {
                    int change = clampedValue - _currentHealth;
                    _currentHealth = clampedValue;
                    OnCurrentHealthChange?.Invoke(change);
                }
            }
        }

        public event System.Action<int> OnCurrentHealthChange;

        //[ShowInInspector, HideInEditorMode, BoxGroup("Game State/Input")]
        //private bool _beginJump;

        [Button, BoxGroup("Helpers")]
        protected float CalcJumpForceFromDistance(float distance)
        {
            return Mathf.Sqrt(-distance * (2 * Physics2D.gravity.y));
        }

        [Button, BoxGroup("Helpers")]
        protected float CalcFallVelocityByTime(float time)
        {
            return Physics2D.gravity.y * _fallMultiplier * time;
        }

        [Button, BoxGroup("Helpers")]
        protected float CalcFallVelocityByDistance(float distance)
        {
            return Mathf.Sqrt(2f * Physics2D.gravity.y * _fallMultiplier * -distance);
        }

        [Button, BoxGroup("Helpers")]
        protected int CalcDamageBySpeed(float speed)
        {
            if (speed <= _fallDamageMinSpeed)
            {
                return 0;
            }
            else
            {
                return Mathf.RoundToInt((speed - _fallDamageMinSpeed) * _damagePerOverspeed);

            }
        }

        protected void OnDestroy()
        {
            Dispose();
        }

        #region Initializers and Destroyers
        // Start is called before the first frame update
        protected void Awake()
        {
            _standColliderSize = _collider.size;
            _standColliderOffset = _collider.offset;

            _crouchColliderSize = new Vector2(_collider.size.x, _collider.size.y * _crouchSizeMultiplier);
            _crouchColliderOffset = new Vector2(_collider.offset.x, _collider.offset.y * _crouchSizeMultiplier);
        }

        // Update is called once per frame
        //void Update()
        //{
        //    UpdateInput();
        //}

        public virtual void Init()
        {
            Debug.Log("Initializing Player...");
            _possibleInteractables = new List<IInteractable>();
            _currentHealth = _baseHealth;
            _input.Init();

            _isAlive = true;

            Debug.Log("Player Initialized!");
        }

        protected void Dispose()
        {
            _input.Dispose();
            _possibleInteractables?.Clear();
        }
        #endregion

        #region Updates
        protected void Update()
        {
            if (_isAlive)
            {
                UpdateInput();

                UpgradeGrounded();

                UpdateAttack();
                UpdateInteract();
                UpdateCrouch();
                UpdateTranslation();
                UpdateJump();
            }
        }

        protected void UpgradeGrounded()
        {
            bool prevoiusIsGrounded = _isGrounded;
            _isGrounded = IsGrounded();

            if (!_isGrounded)
            {
                if (_lastFallSpeed > _fallDamageMaxSpeed)
                {
                    KillByFall();
                }
                else
                {
                    _lastFallSpeed = -_rigidBody.velocity.y;
                }
            }
            else if (!prevoiusIsGrounded)
            {
                //if (_isHangingDown)
                //{
                //    Debug.Log("");
                //    _isHangingDown = false;
                //    EndClimb();
                //}

                if (_currentGround != null)
                {
                    Transform.SetParent(_currentGround.transform);
                }

                LandEffect();

                Debug.Log("Fall at " + _lastFallSpeed + "m/s");
                if (_lastFallSpeed > _fallDamageMinSpeed)
                {
                    TakeDamage(new Damage(CalcDamageBySpeed(_lastFallSpeed)));
                }
                _lastFallSpeed = 0;
            }
        }

        protected void UpdateInput()
        {
            if (Time.time > _stuntEndTime)
            {
                _input.UpdateInput();
            }
        }

        protected void UpdateAttack()
        {
            if (!_isClimbling && !_isAttacking) //TODO; allow attacks while jumping, crouching, climbing and falling
            {
                if (_input.AttackDown || Time.time < _chainAttackEndTime)
                {
                    BeginAttack();
                }
            }
            else
            {
                if (_input.AttackDown)
                {
                    _chainAttackEndTime = Time.time + _chainAttackDuration;
                }
            }
        }

        protected void UpdateInteract()
        {
            if (!_isHangingDown && !_isAttacking)
            {
                if (_input.InteractDown)
                {
                    if (_isCrouch && _currentGround != null && _platformMask.Contains(_currentGround.gameObject.layer))
                    {
                        _isHangingDown = true;
                    }
                    else if (_activeInteractable != null)
                    {
                        _activeInteractable.Interact(this);
                    }
                }
            }
        }

        protected void UpdateTranslation()
        {
            if (!_isAttacking || _isJumping || _isFalling)
            {
                if (_isClimbling)
                {
                    if (_input.MoveLeft || _input.MoveRight)
                    {
                        float deltaSpeed = _climbingSpeed * Time.deltaTime;

                        Vector3 newPosition = Transform.localPosition;
                        if (_input.MoveLeft)
                        {
                            //_renderer.flipX = true;
                            if (!_isClimbling)
                            {
                                _rendererTransform.eulerAngles = new Vector3(0, 180f, 0);
                            }
                            newPosition.x = Transform.localPosition.x + _input.Move.x * deltaSpeed;
                            if (newPosition.x < _ladder.MinPosition.x)
                            {
                                newPosition.x = _ladder.MinPosition.x;
                                EndHang();
                            }
                            else
                            {
                                BeginHang();
                                _animator.SetSpeed(Mathf.Max(0.5f, _input.Move.x));
                            }
                        }
                        else if (_input.MoveRight)
                        {
                            if (!_isClimbling)
                            {
                                _rendererTransform.eulerAngles = Vector3.zero;
                            }
                            newPosition.x = Transform.localPosition.x + _input.Move.x * deltaSpeed;

                            if (newPosition.x > _ladder.MaxPosition.x)
                            {
                                newPosition.x = _ladder.MaxPosition.x;
                                EndLadder();
                            }
                            else
                            {
                                BeginHang();
                                _animator.SetSpeed(Mathf.Max(0.5f, _input.Move.x));
                            }
                        }

                        Transform.localPosition = newPosition;
                    }
                    else
                    {
                        EndHang();
                    }

                    if (_input.MoveDown || _input.MoveUp)
                    {
                        float deltaSpeed = _climbingSpeed * Time.deltaTime;

                        Vector3 newPosition = Transform.localPosition;

                        if (_input.MoveDown)
                        {
                            newPosition.y = Transform.localPosition.y + _input.Move.y * deltaSpeed;

                            if (newPosition.y < _ladder.MinPosition.y)
                            {
                                newPosition.y = _ladder.MinPosition.y;
                                EndLadder();
                            }
                            else
                            {
                                BeginLadder();
                                _animator.SetSpeed(Mathf.Max(0.5f, _input.Move.y));
                            }
                        }
                        else if (_input.MoveUp)
                        {
                            newPosition.y = Transform.localPosition.y + _input.Move.y * deltaSpeed;

                            if (newPosition.y > _ladder.MaxPosition.y)
                            {
                                newPosition.y = _ladder.MaxPosition.y;
                                EndLadder();
                            }
                            else
                            {
                                BeginLadder();
                                _animator.SetSpeed(Mathf.Max(0.5f, _input.Move.y));
                            }
                        }

                        Transform.localPosition = newPosition;
                    }
                    else
                    {
                        EndLadder();
                    }
                }
                else if (_isHangingDown)
                {
                    if (_ladder != null)
                    {
                        if (_input.MoveLeft || _input.MoveRight)
                        {
                            float deltaSpeed = _climbingSpeed * Time.deltaTime;

                            Vector3 newPosition = Transform.localPosition;
                            if (_input.MoveLeft)
                            {
                                //_renderer.flipX = true;
                                _rendererTransform.eulerAngles = new Vector3(0, 180f, 0);
                                newPosition.x = Transform.localPosition.x + _input.Move.x * deltaSpeed;
                                if (newPosition.x < _ladder.MinPosition.x)
                                {
                                    newPosition.x = _ladder.MinPosition.x;
                                }
                            }
                            else if (_input.MoveRight)
                            {
                                _rendererTransform.eulerAngles = Vector3.zero;
                                newPosition.x = Transform.localPosition.x + _input.Move.x * deltaSpeed;

                                if (newPosition.x > _ladder.MaxPosition.x)
                                {
                                    newPosition.x = _ladder.MaxPosition.x;
                                }
                            }

                            Transform.localPosition = newPosition;
                        }
                    }
                }
                else
                {
                    if (_input.MoveLeft || _input.MoveRight)
                    {
                        //_renderer.flipX = _input.MoveLeft;
                        _rendererTransform.eulerAngles = _input.MoveLeft ? new Vector3(0, 180f, 0) : Vector3.zero;

                        if (!_isRunning)
                        {
                            BeginRun();
                        }

                        _animator.SetSpeed(Mathf.Max(0.5f, _input.Move.magnitude));

                        Transform.Translate(new Vector2((_isCrouch ? _crouchSpeed : _speed) * _input.Move.x * Time.deltaTime, 0), Space.Self);
                    }
                    else
                    {
                        if (_isRunning)
                        {
                            EndRun();
                        }
                    }
                }
            }
        }

        protected void UpdateCrouch()
        {
            if (!_isAttacking)
            {
                if (_isCrouch)
                {
                    if (!_input.Crouch || _isFalling || _isClimbling || _isJumping || !_isGrounded || _rigidBody.velocity.y != 0)
                    {
                        EndCrouch();
                    }
                }
                else
                {
                    if (_input.Crouch && _isGrounded && !_isJumping && !_isFalling && !_isClimbling && _rigidBody.velocity.y == 0)
                    {
                        BeginCrouch();
                    }
                }
            }
        }

        protected void UpdateJump()
        {
            if (_isJumping)
            {
                if (_rigidBody.velocity.y > 0)
                {
                    // jumping
                    if (!_input.Jump)
                    {
                        _rigidBody.velocity += new Vector2(0, Physics2D.gravity.y * (_lowJumpMultiplier - 1f) * Time.deltaTime);
                    }
                }
                else if (_rigidBody.velocity.y < 0)
                {
                    if (!_isFalling)
                    {
                        BeginFall();
                    }

                    if (_isGrounded)
                    {
                        EndJump();
                    }
                    else
                    {
                        _rigidBody.velocity += new Vector2(0, Physics2D.gravity.y * (_fallMultiplier - 1f) * Time.deltaTime);
                    }
                }
                else
                {
                    if (_isGrounded)
                    {
                        EndJump();
                    }
                }
            }
            else if (_isHangingDown)
            {
                if (_isGrounded)
                {
                    HangDown();
                }
                else if (Transform.localPosition.y <= _ladder.MaxPosition.y)
                {
                    _isHangingDown = false;
                    _ladder.Interact(this);

                    if (_platform != null)
                    {
                        _platform.Effector.rotationalOffset = 0;
                        _platform = null;
                    }
                }
                else
                {
                    _rigidBody.velocity += new Vector2(0, Physics2D.gravity.y * (_fallMultiplier - 1f) * Time.deltaTime);
                }
            }
            else if (_isFalling)
            {
                if (_isGrounded)
                {
                    EndFall();
                }
            }
            else if (!_isAttacking)
            {
                if (_isClimbling)
                {
                    if (_input.JumpDown)
                    {
                        EndClimb();

                        if (_input.Move.y < -0.5f)
                        {
                            JumpDown();
                        }
                        else
                        {
                            BeginJump(); // TODO lateral and down jamp?
                        }
                    }
                    else if (Transform.localPosition.x < _ladder.MinPosition.x ||
                            Transform.localPosition.x > _ladder.MaxPosition.x ||
                            Transform.localPosition.y < _ladder.MinPosition.y ||
                            Transform.localPosition.y > _ladder.MaxPosition.y)
                    {
                        EndClimb();
                    }
                }
                else if (_isGrounded)
                {
                    if (_isCrouch && _currentGround != null && _platformMask.Contains(_currentGround.gameObject.layer))
                    {
                        if (_input.JumpDown)
                        {
                            JumpDown();
                        }
                    }
                    else if (_input.JumpDown)
                    {
                        BeginJump();
                    }
                }
                else
                {
                    BeginFall();
                }
            }
        }
        #endregion

        #region Change States
        protected void JumpDown()
        {
            if (_platform == null && _currentGround != null)
            {
                _platform = _currentGround.GetComponent<Platform>();
                _platform.Effector.rotationalOffset = 180f;
                _isGrounded = false;
            }

            Timing.CallDelayed(0.5f, () =>
            {
                if (_platform != null)
                {
                    _platform.Effector.rotationalOffset = 0;
                    _platform = null;
                }
            });
        }

        protected void HangDown()
        {
            if (_platform == null && _currentGround != null)
            {
                _platform = _currentGround.GetComponent<Platform>();
                _platform.Effector.rotationalOffset = 180f;
                _isGrounded = false;

                //_rigidBody.velocity = new Vector2(0, -_hangingDownForce);

                if (_platform.Hanger != null)
                {
                    _animator.BeginClimb();
                    _animator.SetSpeed(0);

                    _ladder = _platform.Hanger;
                }
                else
                {
                    _isHangingDown = false;
                }
            }

            Timing.CallDelayed(0.5f, () =>
            {
                if (_platform != null)
                {
                    _platform.Effector.rotationalOffset = 0;
                    _platform = null;

                    if (_isHangingDown)
                    {
                        _isHangingDown = false;
                        EndClimb();
                    }
                }
            });
        }

        protected bool IsGrounded()
        {
            if (_isClimbling)
            {
                return false;
            }

            if (_rigidBody.velocity.y == 0)
            {
                if (_currentGround != null)
                {
                    return true;
                }
                else
                {
                    RaycastHit2D raycastHit = Physics2D.BoxCast(
                        new Vector2(_collider.bounds.center.x, _collider.bounds.center.y - _collider.bounds.size.y / 2f),
                        new Vector2(_collider.bounds.size.x, _collider.bounds.size.y / 2f), 
                        0, 
                        Vector2.down, 
                        _checkGroundOffset, 
                        _groundMask);

                    _currentGround = raycastHit.collider;

#if UNITY_EDITOR
                    Color rayColor = _currentGround == null ? Color.red : Color.green;
                    Debug.DrawRay(new Vector2(_collider.bounds.center.x, _collider.bounds.center.y - _collider.bounds.size.y / 2f), Vector2.down * (_collider.bounds.extents.y + _checkGroundOffset), rayColor);
#endif
                    return true;
                }
            }
            else
            {
                RaycastHit2D raycastHit = Physics2D.BoxCast(
                        new Vector2(_collider.bounds.center.x, _collider.bounds.center.y - _collider.bounds.size.y),
                        new Vector2(_collider.bounds.size.x, 0),
                        0,
                        Vector2.down,
                        _checkGroundOffset,
                        _groundMask);

                _currentGround = raycastHit.collider;

#if UNITY_EDITOR
                Color rayColor = _currentGround == null ? Color.red : Color.green;
                Debug.DrawRay(new Vector2(_collider.bounds.center.x, _collider.bounds.center.y - _collider.bounds.size.y / 2f), Vector2.down * (_collider.bounds.extents.y + _checkGroundOffset), rayColor);
#endif

                //if (_platform != null && _currentGround != null && _currentGround.gameObject == _platform.gameObject)
                //{
                //    return false;
                //}

                return false; // raycastHit.collider != null;
            }
        }

        #endregion

        #region Actions States
        protected void BeginRun()
        {
            _isRunning = true;
            _animator.BeginRun();
        }

        protected void EndRun()
        {
            _isRunning = false;
            _animator.EndRun();
        }

        protected void BeginJump()
        {
            if (!_isJumping)
            {
                _isJumping = true;
                Transform.SetParent(null);
                _animator.BeginJump();
                JumpEffect();

                _rigidBody.velocity = new Vector2(0, _jumpForce);
            }

            //Physics2D.GetLayerCollisionMask
        }

        protected void EndJump()
        {
            _isJumping = false;
            _animator.EndJump();
        }

        protected void BeginFall()
        {
            _isFalling = true;
            Transform.SetParent(null);
            _animator.BeginFall();
        }

        protected void EndFall()
        {
            _isFalling = false;
            _animator.EndFall();

            if (_platform != null)
            {
                _platform.Effector.rotationalOffset = 0;
                _platform = null;
            }
        }

        protected void BeginCrouch()
        {
            if (!_isCrouch)
            {
                _isCrouch = true;
                _animator.BeginCrouch();

                Bounds bounds = _collider.bounds;

                _collider.size = _crouchColliderSize;
                _collider.offset = _crouchColliderOffset;
            }
        }

        protected void EndCrouch()
        {
            if (_isCrouch)
            {
                _isCrouch = false;
                _animator.EndCrouch();

                _collider.size = _standColliderSize;
                _collider.offset = _standColliderOffset;
            }
        }

        public void SwitchClimb(Ladder ladder)
        {
            if (_isClimbling)
            {
                EndClimb();
            }
            else
            {
                if (_isFalling)
                {
                    EndFall();
                }
                BeginClimb(ladder);
            }
        }

        public void BeginClimb(Ladder ladder)
        {
            _isClimbling = true;
            _currentGround = null;

            _ladder = ladder;
            Transform.SetParent(_ladder.Transform);
            _rigidBody.gravityScale = 0;
            //_rigidBody.bodyType = RigidbodyType2D.Kinematic; //TODO: Fix con esto no choca con obst�culos. �Crear f�sicas propias?
            _rigidBody.velocity = Vector2.zero;
            _ladder.Attach(_rigidBody);

            if (_isJumping)
            {
                EndJump();
            }
            if (_isRunning)
            {
                EndRun();
            }

            _rendererTransform.eulerAngles = Vector3.zero;
            _animator.BeginClimb();

            Vector3 newPosition = Transform.localPosition;
            if (newPosition.x < _ladder.MinPosition.x)
            {
                newPosition.x = _ladder.MinPosition.x;
            }
            else if (newPosition.x > _ladder.MaxPosition.x)
            {
                newPosition.x = _ladder.MaxPosition.x;
            }

            if (newPosition.y < _ladder.MinPosition.y)
            {
                newPosition.y = _ladder.MinPosition.y;
            }
            else if (newPosition.y > _ladder.MaxPosition.y)
            {
                newPosition.y = _ladder.MaxPosition.y;
            }

            Transform.localPosition = newPosition;
        }

        public void EndClimb()
        {
            if (_isHanging)
            {
                _isHanging = false;
                _animator.EndHang();
            }
            if (_isLadding)
            {
                _isLadding = false;
                _animator.EndLadder();
            }

            _isClimbling = false;

            _ladder.Detach();
            _ladder = null;

            //_rigidBody.velocity = Vector2.zero;
            _rigidBody.gravityScale = 1;

            //_rigidBody.bodyType = RigidbodyType2D.Dynamic;

            _animator.EndClimb();
        }

        public void BeginHang()
        {
            if (!_isHanging)
            {
                _isHanging = true;
                _animator.BeginHang();
            }
        }

        public void EndHang()
        {
            if (_isHanging)
            {
                _isHanging = false;
                _animator.EndHang();
                _animator.SetSpeed(1f);
            }
        }

        public void BeginLadder()
        {
            if (!_isLadding)
            {
                _isLadding = true;
                _animator.BeginLadder();
            }
        }

        public void EndLadder()
        {
            if (_isLadding)
            {
                _isLadding = false;
                _animator.EndLadder();
                _animator.SetSpeed(1f);
            }
        }

        public void BeginAttack()
        {
            if (_isRunning)
            {
                EndRun();
            }

            _isAttacking = true;
            _chainAttackEndTime = 0;

            Attack attack;
            if (_isJumping || _isFalling)
            {
                attack = _airAttack;
                _animator.BeginAttack();
            }
            else if (_isCrouch)
            {
                attack = _crouchAttack;
                _animator.BeginAttack();
            }
            else
            {
                if (_attackComboEndTime > 0)
                {
                    if (Time.time > _attackComboEndTime || ++_attackCombo >= _attacks.Length)
                    {
                        _attackCombo = 0;
                        _attackComboEndTime = 0;
                    }
                }

                attack = _attacks[_attackCombo];
                _animator.SetAttackCombo(_attackCombo);
                _animator.BeginAttack();

                _attackComboEndTime = Time.time + _chainAttackDuration;
            }

            Timing.CallDelayed(attack.Duration, EndAttack);
        }

        private void EndAttack()
        {
            _isAttacking = false;
            _animator.EndAttack();
        }

        #endregion

        #region Triggers

        // TODO: Order by distance
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isAlive)
            {
                ITriggerable triggereable = collision.GetComponent<ITriggerable>();
                if (triggereable != null)
                {
                    triggereable.EnterTrigger(this);
                }

                IInteractable interactable = collision.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    _possibleInteractables.Add(interactable);

                    if (_activeInteractable == null)
                    {
                        _activeInteractable = interactable;
                        _activeInteractable.CanInteract(this, true);
                    }
                    else
                    {
                        if (interactable.Priority > _activeInteractable.Priority)
                        {
                            _activeInteractable.CanInteract(this, false);
                            _activeInteractable = interactable;
                            _activeInteractable.CanInteract(this, true);
                        }
                    }

                    return;
                }
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (_isAlive)
            {
                ITriggerable triggereable = collision.GetComponent<ITriggerable>();
                if (triggereable != null)
                {
                    triggereable.ExitTrigger();
                }

                IInteractable interactable = collision.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    _possibleInteractables.Remove(interactable);

                    if (_activeInteractable == interactable)
                    {
                        _activeInteractable.CanInteract(this, false);

                        // Search for anoher interactable
                        if (_possibleInteractables.Count == 0)
                        {
                            // No more interactables
                            _activeInteractable = null;
                            if (_activeInteractable is Ladder && _ladder == (Ladder)_activeInteractable)
                            {
                                _ladder = null;
                            }
                        }
                        else if (_possibleInteractables.Count == 1)
                        {
                            // Only one interactable
                            if (_activeInteractable is Ladder && _ladder == (Ladder)_activeInteractable)
                            {
                                if (_possibleInteractables[0] is Ladder)
                                {
                                    _ladder = (Ladder)_possibleInteractables[0];
                                }
                                else
                                {
                                    _ladder = null;
                                }
                            }

                            _activeInteractable = _possibleInteractables[0];
                            _activeInteractable.CanInteract(this, true);
                        }
                        else if (_possibleInteractables.Count > 1)
                        {
                            // More than one interactables
                            if (_activeInteractable is Ladder && _ladder == (Ladder)_activeInteractable)
                            {
                                for (int i = 1; i < _possibleInteractables.Count; ++i)
                                {
                                    if (_possibleInteractables[i] is Ladder)
                                    {
                                        _activeInteractable = _possibleInteractables[i];
                                        _ladder = (Ladder)_possibleInteractables[i];
                                        break;
                                    }
                                }
                                _ladder = null;
                            }

                            if (_ladder == null)
                            {
                                _activeInteractable = _possibleInteractables[0];
                                for (int i = 1; i < _possibleInteractables.Count; ++i)
                                {
                                    if (_possibleInteractables[i].Priority > _activeInteractable.Priority)
                                    {
                                        _activeInteractable = _possibleInteractables[i];
                                    }
                                }

                                _activeInteractable.CanInteract(this, true);
                            }
                        }
                    }
                }
            }
        }

        protected void ApplyForce(Transform origin, float force)
        {
            if (origin != null)
            {
                if (origin.position.x > Transform.position.x)
                {
                    _rigidBody.velocity = new Vector2(-force, _rigidBody.velocity.y);
                    return;
                }
                else if (origin.position.x < Transform.position.x)
                {
                    _rigidBody.velocity = new Vector2(force, _rigidBody.velocity.y);
                    return;
                }
            }

            // Si no hay origen definido o está en la misma x que el personaje, empuja en dirección contraria a la que mira
            if (_rendererTransform.eulerAngles.y == 0)
            {
                _rigidBody.velocity = new Vector2(-force, _rigidBody.velocity.y);
            }
            else
            {
                _rigidBody.velocity = new Vector2(force, _rigidBody.velocity.y);
            }
        }

        public virtual void TakeDamage(Damage damage, Transform origin = null)
        {
            if (damage.IgnoreInvulnerability || Time.time >= _lastDamageTakenTime + _invulnerabilityTimeAfterTakeDamage)
            {
                _lastDamageTakenTime = Time.time;

                if (_isAttacking)
                {
                    EndAttack();
                }

                if (_isClimbling)
                {
                    EndClimb();
                }

                if (damage.StuntDuration > 0)
                {
                    _stuntEndTime = Mathf.Max(_stuntEndTime, _lastDamageTakenTime + damage.StuntDuration);
                    _input.CleanInput();
                }

                if (damage.PushForce != 0)
                {
                    ApplyForce(origin, damage.PushForce);
                }

                _animator.TriggerHurt();

                CurrentHealth -= damage.Amount;
                if (_currentHealth <= 0)
                {
                    Kill();
                }
                else
                {
                    TakeDamageEffect(origin, damage);
                }
            }
        }

        protected virtual void TakeDamageEffect(Transform origin, Damage damage)
        {

        }

        protected virtual void JumpEffect()
        {

        }

        protected virtual void LandEffect()
        {

        }

        protected virtual void Kill()
        {

        }

        protected virtual void KillByFall()
        {

        }
        #endregion
    }
}