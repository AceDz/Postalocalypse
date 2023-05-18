using Sirenix.OdinInspector;
using UnityEngine;

namespace TeamTheDream.Delivery
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField, BoxGroup("Horizontal Movement")] private float _moveSpeed = 10f;
        [SerializeField, BoxGroup("Jump")] private float _jumpSpeed = 9f;
        [SerializeField, BoxGroup("Jump")] private float _doubleJumpSpeed = 18f;
        [SerializeField, BoxGroup("Jump")] private float _wallJumpSpeed = 9f;
        [SerializeField, BoxGroup("Jump")] private float _wallJumpTimer = 1f;
        [SerializeField, BoxGroup("Jump")] private float _MaxTimeGliding = 0.7f;
        [SerializeField, BoxGroup("Jump")] private float _wallSlidingSpeed = 0.2f;
        [SerializeField, BoxGroup("Jump")] private float _gravityAument = 2;
        [SerializeField, BoxGroup("Sensors")] private Transform _feet;
        [SerializeField, BoxGroup("Sensors")] private Transform _rightSensor;
        [SerializeField, BoxGroup("Sensors")] private Transform _leftSensor;
        [SerializeField, BoxGroup("LayerMasks")] private LayerMask _layerMask;
        [SerializeField, BoxGroup("LayerMasks")] private LayerMask _layerMaskWall;
        [SerializeField, BoxGroup("Skills")] private SkillScriptableObject _wallJumpSkill;
        [SerializeField, BoxGroup("Skills")] private SkillScriptableObject _doubleJumpSkill;
        [SerializeField, BoxGroup("Skills")] private SkillScriptableObject _fireSystemSkill;
        [SerializeField, BoxGroup("FireSystem")] private Transform _aimPoint;
        [SerializeField, BoxGroup("FireSystem")] private GameObject _gun;
        [SerializeField, BoxGroup("ParticleSystem")] private ParticleSystem _jumpParticle;
        [SerializeField, BoxGroup("ParticleSystem")] private ParticleSystem _hitParticle;

        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private Vector2 _wallJumpDirection = Vector2.down;
        private float _initialGravity;
        private float _wallJumpingTime;
        private float _glidingTime = 0;
        private bool _gravityChanged = false;
        private bool _canDoubleJump;
        private bool _isMoveBlocked;
        public bool IsGrounded;
        private Transform groundedObject;
        private Vector3? groundedObjectLastPosition;
        private Animator _animator;


        public Transform AimPoint => _aimPoint;
        
        public ParticleSystem HitParticle => _hitParticle;
            
        private void Awake()
        {
            Cursor.visible = false;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _initialGravity = _rigidbody2D.gravityScale;
            _fireSystemSkill.OnSkillStateChange += ActivateFireSystem;
            _animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            _gun.SetActive(_fireSystemSkill.SkillUnlocked);
            _aimPoint.gameObject.SetActive(_fireSystemSkill.SkillUnlocked);
        }
        void Update()
        {
            if (_isMoveBlocked)
            {
                return;
            }
                
            var horizontal = Input.GetAxis("Horizontal");
            PlayerGrounding();
            if (_wallJumpingTime <= 0)
            {
                HorizontalMovement(horizontal);
            }

            if (IsGrounded)
            {
                _glidingTime = 0;
                _gravityChanged = false;
                _rigidbody2D.gravityScale = _initialGravity;
                
                if (Input.GetButtonDown("Jump"))
                {
                     Jump();
                }
            }
            else
            {
                _glidingTime += Time.deltaTime;
                if (_glidingTime > _MaxTimeGliding)
                {
                    _gravityChanged = true;
                    _rigidbody2D.gravityScale = _gravityAument;
                }

            }

            if (CheckIfCanDoubleJump())
            {
                DoubleJump();
            }

            if (Input.GetButtonUp("Jump") && !_gravityChanged)
            {
                _gravityChanged = true;
                _rigidbody2D.gravityScale = _gravityAument;
            }

            if (!IsGrounded && ShouldSlide(horizontal))
            {
                Slide();
                if (Input.GetButtonDown("Jump") && _wallJumpSkill.SkillUnlocked)
                {
                    WallJump();
                }

            }

            if (_wallJumpingTime > 0)
            {
                _wallJumpingTime -= Time.deltaTime;
            }

            if (!_fireSystemSkill.SkillUnlocked)
            {
                if (horizontal >= 0)
                {
                    _spriteRenderer.flipX = false;
                } else
                {
                    _spriteRenderer.flipX = true;
                }
            }

            _animator.SetBool("IsJumping", !IsGrounded);
            StickToMovingObjects();
        }

        private bool CheckIfCanDoubleJump()
        {
            return !IsGrounded && _canDoubleJump && _doubleJumpSkill.SkillUnlocked && (Input.GetButtonDown("Jump"));
        }

        private void DoubleJump()
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _doubleJumpSpeed);
            _canDoubleJump = false;
        }

        

        private void Slide()
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, Mathf.Clamp(_rigidbody2D.velocity.y, -_wallSlidingSpeed, float.MaxValue));
        }

        private bool ShouldSlide(float horizontal)
        {
            if (Physics2D.OverlapCircle(_rightSensor.position, 0.1f, _layerMaskWall) && horizontal > 0)
            {
                _wallJumpDirection = Vector2.left;
                return true;
            }

            if (Physics2D.OverlapCircle(_leftSensor.position, 0.1f, _layerMaskWall) && horizontal < 0)
            {
                _wallJumpDirection = Vector2.right;
                return true;
            }

            return false;
        }

        private void StickToMovingObjects()
        {
            if (groundedObject != null)
            {
                if (groundedObjectLastPosition.HasValue &&
                    groundedObjectLastPosition != groundedObject.position)
                {
                    Vector3 delta = groundedObject.position - groundedObjectLastPosition.Value;
                    transform.position += delta;
                }

                groundedObjectLastPosition = groundedObject.position;
            }
            else
            {
                groundedObjectLastPosition = null;
            }
        }
        private void PlayerGrounding()
        {
            var raycastHit = Physics2D.OverlapCircle(_feet.position, 0.2f, _layerMask);
            if (raycastHit != false)
            {
                if (groundedObject != raycastHit.GetComponent<Collider2D>().transform)
                {
                    groundedObjectLastPosition = raycastHit.GetComponent<Collider2D>().transform.position;
                }
                IsGrounded = true;
                groundedObject = raycastHit.GetComponent<Collider2D>().transform;
            }
            else
            {
                IsGrounded = false;
                groundedObject = null;
            }

        }

        private void Jump()
        {
            _canDoubleJump = true;
            CreateJumpParticle();
            _animator.SetTrigger("Jump");           
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpSpeed);
        }
        private void WallJump()
        {
            _canDoubleJump = true;
            _rigidbody2D.velocity = new Vector2(_moveSpeed * _wallJumpDirection.x, _wallJumpSpeed);
            _rigidbody2D.gravityScale = _initialGravity;
            _wallJumpingTime = _wallJumpTimer;
        }

        private void HorizontalMovement(float horizontal)
        {
            //float smoothnessMultiplier = horizontal == 0 ? _decceleration : _acceleration;
            var desiredVelocity = horizontal * _moveSpeed;
            //desiredVelocity = Mathf.Lerp(_rigidbody2D.velocity.x, desiredVelocity, smoothnessMultiplier);
            _rigidbody2D.velocity = new Vector2(desiredVelocity, _rigidbody2D.velocity.y);
        }

        public void ActivateFireSystem()
        {
            _aimPoint.localPosition = Vector3.zero;
            _gun.SetActive(_fireSystemSkill.SkillUnlocked);
            _aimPoint.gameObject.SetActive(_fireSystemSkill.SkillUnlocked);
        }

        private void DeactivateFireSystem()
        {
            _gun.SetActive(false);
            _aimPoint.gameObject.SetActive(false);
            _aimPoint.localPosition = new Vector3(0, 3f, 0);
        }

        private void CreateJumpParticle()
        {
            _jumpParticle.Play();
        }

        public void Die()
        {
            Stop();
        }

        public void Revive()
        {
            GetComponent<HealthManager>().Revive();
            Resume();
        }

        public void Stop()
        {
            _isMoveBlocked = true;
            _rigidbody2D.velocity = new Vector2(0, IsGrounded ? 0 : _rigidbody2D.velocity.y);
            _rigidbody2D.angularVelocity = 0;
            GetComponent<HealthManager>().enabled = false;
            DeactivateFireSystem();
        }

        public void Resume()
        {
            _isMoveBlocked = false;
            GetComponent<HealthManager>().enabled = true;
            ActivateFireSystem();
        }
    }

}
