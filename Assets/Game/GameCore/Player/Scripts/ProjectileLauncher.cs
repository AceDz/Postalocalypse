using Sirenix.OdinInspector;
using UnityEngine;

namespace TeamTheDream.Delivery
{
    public class ProjectileLauncher : MonoBehaviour
    {
        [SerializeField, BoxGroup("GameObjects")] private Projectile _prefab;
        [SerializeField, BoxGroup("GameObjects")] private Transform _aimPoint;
        [SerializeField, BoxGroup("Aiming")] private float aimMoveSpeed = 5f;
        [SerializeField, BoxGroup("Shooting")] private float _projectileForce = 3f;
        [SerializeField, BoxGroup("Shooting")] private float _projectileGravityForceCompensation = 5f;
        [SerializeField, BoxGroup("Shooting")] private float _shootCooldown = 1;
        [SerializeField, BoxGroup("Shooting")] private float _distanceDivider = 1;
        [SerializeField, BoxGroup("Shooting")] private int _damage = 1;
        [SerializeField, BoxGroup("Flip Sprite")] private SpriteRenderer _playerSpriteRenderer;
        [SerializeField, BoxGroup("Flip Sprite")] private Transform _rightSide;
        [SerializeField, BoxGroup("Flip Sprite")] private Transform _leftSide;
        [SerializeField, BoxGroup("Items")] private ExtraDamageItem _extraDamageItem;
        [SerializeField, BoxGroup("Items")] private QuickShootItem _quickShootItem;
        [SerializeField, BoxGroup("Audio")] private AudioManager _audioManager;
        
        private PlayerMovementController _player;
        private PlayerAnimationController _animatorController;
        private bool _forceCheckFacing = true;
        private bool _isFacingRight = true;
        private float shootTimer = 0;

        public int Damage { get => _damage; set { _damage = value; } }


        private void Awake()
        {
            _player = GetComponentInParent<PlayerMovementController>();
            _animatorController = GetComponentInParent<PlayerAnimationController>();
        }

        private void OnEnable()
        {
            _extraDamageItem.OnAmountChanged += AddExtraDamage;
            _quickShootItem.OnAmountChanged += AddQuickShot;
        }

        private void OnDisable()
        {
            _forceCheckFacing = true;
            _extraDamageItem.OnAmountChanged -= AddExtraDamage;
            _quickShootItem.OnAmountChanged -= AddQuickShot;
        }

        private void AddQuickShot(int amount)
        {
            _shootCooldown = _shootCooldown / 2f;
        }

        private void AddExtraDamage(int amount)
        {
            _damage++;
        }

        private void Update()
        {
            if (!IsGamepadConnected())
            {
                if (Camera.main == null)
                    return;
                
                var mousePosition = Input.mousePosition;
                var targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                _aimPoint.position = new Vector3(targetPosition.x, targetPosition.y, 1);

                ArmRotationAndPlayerFlip();
            }
            else
            {
                var horizontal = Input.GetAxis("AimHorizontal");
                var vertical = -Input.GetAxis("AimVertical");
                Vector3 moveDirection = new Vector3(horizontal, vertical, 0f);
                _aimPoint.position += (moveDirection * Time.deltaTime * aimMoveSpeed);

                ArmRotationAndPlayerFlip();
            }

            if (Input.GetButtonDown("Fire1") && shootTimer <= 0)
            {
                Shoot();
            }

            if (shootTimer > 0)
                shootTimer -= Time.deltaTime;
        }

        private void Shoot()
        {
            StartCoroutine(_animatorController.LaunchAttackAnimation());
            shootTimer = _shootCooldown;
            var distance = Vector3.Distance(transform.position, _aimPoint.position);
            var distanceForce = distance / _distanceDivider;
            var direction = _aimPoint.position - transform.position;
            direction.Normalize();
            var force = direction * _projectileForce * distanceForce;
            force.z = 0;
            force.y += _projectileGravityForceCompensation;
            var projectile = Instantiate(_prefab, transform.position, transform.rotation);
            projectile.Setup(force, _damage, _audioManager);

            Destroy(projectile, 5);
        }

        private void ArmRotationAndPlayerFlip()
        {
            var direction = _aimPoint.position - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            var playerToAimPoint = _aimPoint.position - _player.transform.position;
            playerToAimPoint.Normalize();
            float playerToAimPointAngle = Mathf.Atan2(playerToAimPoint.y, playerToAimPoint.x) * Mathf.Rad2Deg;
            if (Mathf.Abs(playerToAimPointAngle) > 90f && (_isFacingRight || _forceCheckFacing))
            {
                _isFacingRight = false;
                _playerSpriteRenderer.flipX = true;
                transform.position = _leftSide.position;
                angle += 180;

            }
            else if (Mathf.Abs(playerToAimPointAngle) < 90f && (!_isFacingRight || _forceCheckFacing))
            {
                _playerSpriteRenderer.flipX = false;
                _isFacingRight = true;
                transform.position = _rightSide.position;
            }

            angle += 90;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            _forceCheckFacing = false;
        }

        public bool IsGamepadConnected()
        {
            string[] joysticks = Input.GetJoystickNames();
            foreach (var joystick in joysticks)
            {
                if (!string.IsNullOrEmpty(joystick))
                    return true;
            }
            return false;
        }


    }
}

