using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TeamTheDream.Delivery
{
    public class EnemyPatrolMovement : SerializedMonoBehaviour
    {
        [SerializeField, BoxGroup("Patrolling Points")]
        private Transform _patrolPointTransform;

        [SerializeField, BoxGroup("Enemy")] private Transform _enemyTransform;

        [SerializeField, BoxGroup("Enemy")] private float _enemySpeed;
        
        [SerializeField, BoxGroup("Chasing Player")] private Vector2 _chasingRange;
        [SerializeField, BoxGroup("Chasing Player")] private Vector2 _chasingRangeOffset;
        [SerializeField, BoxGroup("Chasing Player")] private float _chasingSpeed;


        private Vector3 _initScale;
        private Vector3 _initPosition;
        private Vector3 _initLeftPoint;
        private Vector3 _initRightPoint;
        
        private bool _movingLeft;
        private bool _isPatrolling;

        private Enemy _enemy;
        private Transform _playerTransform;

        private void Awake()
        {
            _enemy = GetComponentInParent<Enemy>(); 
            _playerTransform = FindObjectOfType<PlayerMovementController>(true).transform;
            _initScale = _enemyTransform.localScale;
            _initPosition = _enemyTransform.position;
            _isPatrolling = true;

            if (_patrolPointTransform == null)
            {
                _isPatrolling = false;
                return;
            }

            if (_enemyTransform.position.x > _patrolPointTransform.position.x)
            {
                _initLeftPoint = _patrolPointTransform.position;
                _initRightPoint = _enemyTransform.position;
                _movingLeft = true;
            }
            else
            {
                _initRightPoint = _patrolPointTransform.position;
                _initLeftPoint = _enemyTransform.position;
                _movingLeft = false;
            }
        }

        private void Update()
        {
            if (!_enemy.IsAlive || _enemy.IsHurting || _enemy.IsAttacking)
                return;

            if (IsPlayerInChasingRange())
            {
                UpdateChasePlayer();
                return;
            }
            if (_isPatrolling)
            {
                UpdatePatrol();
            }
            
        }

        private bool IsPlayerInChasingRange()
        {
            if (_playerTransform == null)
                return false;

            var chasingCheckPosition = (Vector2)_enemyTransform.position + _chasingRangeOffset;
            var playerPosition = _playerTransform.position;
            
            return Mathf.Abs(chasingCheckPosition.x - playerPosition.x) < _chasingRange.x
                   && Mathf.Abs(chasingCheckPosition.y - playerPosition.y) < _chasingRange.y;
            
        }

        private void UpdateChasePlayer()
        {
            _movingLeft = _enemyTransform.position.x > _playerTransform.position.x;
            
            var direction = _movingLeft ? -1 : 1;
            
            MoveInDirection(direction, _chasingSpeed);

        }
        
        private void UpdatePatrol()
        {
            if (_movingLeft)
            {
                if (_enemyTransform.position.x >= _initLeftPoint.x)
                {
                    MoveInDirection(-1, _enemySpeed);
                }
                else
                {
                    DirectionChange();
                }
                
            }
            else
            {
                if (_enemyTransform.position.x <= _initRightPoint.x)
                {
                    MoveInDirection(1, _enemySpeed);
                }
                else
                {
                    DirectionChange();
                }
            }
        }

        private void DirectionChange()
        {
            _movingLeft = !_movingLeft;
        }

        private void MoveInDirection(float direction, float speed)
        {
            //Face direction
            _enemyTransform.localScale = new Vector3(Mathf.Abs(_initScale.x) * direction, _initScale.y, _initScale.z);
            
            //Move 
            var position = _enemyTransform.position;
            position =
                new Vector3(position.x + Time.deltaTime * direction * speed,
                    position.y, position.z);
            _enemyTransform.position = position;
        }

        private void OnDrawGizmos()
        {
            if (_patrolPointTransform == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(_enemyTransform.position, _patrolPointTransform.position);
        }
    }
}