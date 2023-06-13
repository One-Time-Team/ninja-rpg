using System.Collections;
using Core.Enums;
using Core.Parallax;
using Core.Services.Updater;
using ItemsSystem;
using NPC.Behaviour;
using Pathfinding;
using StatsSystem.Data;
using StatsSystem.Enums;
using UnityEngine;

namespace NPC.Controller
{
    public class MeleeEntity : Entity
    {
        private const float PathUpdateTime = 0.5f;
        private const float NextWayPointDistance = 0.5f;
        private const float TargetHeightRequirementCoef = 1.2f;

        private readonly Seeker _seeker;
        private readonly MeleeEntityBehaviour _meleeEntityBehaviour;
        private readonly float _moveSpeed;
        private readonly DropGenerator _dropGenerator;
        private readonly int _dropAmount;
        
        private bool _isAttacking;
        private Coroutine _searchCoroutine;
        private Collider2D _target;
        private Vector3 _destination;
        private float _stoppingDistance;
        private Path _currentPath;
        private int _currentWayPoint;


        public MeleeEntity(MeleeEntityBehaviour meleeEntityBehaviour, IStatValueGiver statValueGiver, 
            IParallaxTargetMovement parallaxPlayerMovement, DropGenerator dropGenerator, int dropAmount) :
            base(meleeEntityBehaviour, statValueGiver, parallaxPlayerMovement)
        {
            _seeker = meleeEntityBehaviour.GetComponent<Seeker>();
            _meleeEntityBehaviour = meleeEntityBehaviour;
            _dropGenerator = dropGenerator;
            _dropAmount = dropAmount;
            _meleeEntityBehaviour.AttackSequenceEnded += OnAttackEnded;
            _searchCoroutine = ProjectUpdater.Instance.StartCoroutine(SearchCoroutine());
            ProjectUpdater.Instance.FixedUpdateCalled += OnFixedUpdateCalled;
            _moveSpeed = StatValueGiver.GetValue(StatType.Speed) * parallaxPlayerMovement.ParallaxSpeedCoef;
            VisualiseHP(StatValueGiver.GetValue(StatType.Health));
            _meleeEntityBehaviour.AttackImpacted += OnAttack;
            _meleeEntityBehaviour.Disappeared += OnDisappeared;
        }

        public override void Dispose()
        {
            base.Dispose();
            _meleeEntityBehaviour.AttackSequenceEnded -= OnAttackEnded;
            ProjectUpdater.Instance.FixedUpdateCalled -= OnFixedUpdateCalled;
            _meleeEntityBehaviour.AttackImpacted -= OnAttack;
        }
        
        protected sealed override void VisualiseHP(float currentHP)
        {
            if (_meleeEntityBehaviour.HPBar.maxValue < currentHP)
                _meleeEntityBehaviour.HPBar.maxValue = currentHP;

            _meleeEntityBehaviour.HPBar.value = currentHP;
        }

        private IEnumerator SearchCoroutine()
        {
            while (!_isAttacking)
            {
                if (!TryGetTarget(out _target))
                {
                    ResetMovement();
                }
                else if(_meleeEntityBehaviour.FollowEnabled && _seeker.IsDone())
                {
                    Vector2 position = _target.transform.position;
                    _stoppingDistance = (_target.bounds.size.x + _meleeEntityBehaviour.Size.x);
                    var delta = position.x < _meleeEntityBehaviour.transform.position.x ? 1 : -1;
                    _destination = position + new Vector2(_stoppingDistance * delta, 0);
                    _seeker.StartPath(_meleeEntityBehaviour.transform.position, _destination, OnPathCalculated);
                }
                yield return new WaitForSeconds(PathUpdateTime);
            }
        }
        
        private bool TryGetTarget(out Collider2D target)
        {
            target = Physics2D.OverlapBox(_meleeEntityBehaviour.transform.position, _meleeEntityBehaviour.TargetSearchBox, 0,
                _meleeEntityBehaviour.Target);

            return target != null;
        }

        private void OnPathCalculated(Path path)
        {
            if (path.error)
                return;

            _currentPath = path;
            _currentWayPoint = 0;
        }
        
        private void OnFixedUpdateCalled()
        {
            if (!_meleeEntityBehaviour.FollowEnabled || _isAttacking || _target == null || _currentPath == null ||
                CheckIfCanAttack() || _currentWayPoint >= _currentPath.vectorPath.Count) 
                return;

            var currentPosition = _meleeEntityBehaviour.transform.position;
            var waypointPosition = _currentPath.vectorPath[_currentWayPoint];
            var waypointDirection = (waypointPosition - currentPosition);
            
            RaycastHit2D isGrounded = Physics2D.BoxCast(_meleeEntityBehaviour.Center, _meleeEntityBehaviour.Size,
                0, Vector2.down, 0.1f, _meleeEntityBehaviour.Ground);
            RaycastHit2D isObstacleRight = Physics2D.Raycast(_meleeEntityBehaviour.Position,
                Vector2.right, 1f, _meleeEntityBehaviour.Ground);
            RaycastHit2D isObstacleLeft = Physics2D.Raycast(_meleeEntityBehaviour.Position,
                Vector2.left, 1f, _meleeEntityBehaviour.Ground);
            
            if (_meleeEntityBehaviour.JumpEnabled && isGrounded && _target.TryGetComponent(out Rigidbody2D targetRb))
            {
                if ((_target.transform.position.y - TargetHeightRequirementCoef > currentPosition.y && targetRb.velocity.y == 0
                    && _currentPath.path.Count < 20) || ((waypointDirection.x >= 0) && isObstacleRight)
                                                     || ((waypointDirection.x <= 0) && isObstacleLeft))
                    _meleeEntityBehaviour.Jump(StatValueGiver.GetValue(StatType.JumpForce));
            }
            
            _meleeEntityBehaviour.MoveHorizontally(waypointDirection.x * _moveSpeed);

            if (Vector2.Distance(waypointPosition, currentPosition) < NextWayPointDistance)
            {
                _currentWayPoint++;
            }
        }

        private bool CheckIfCanAttack()
        {
            var distance = _destination - _meleeEntityBehaviour.transform.position;
            if (Mathf.Abs(distance.x) >  _stoppingDistance || Mathf.Abs(distance.y) >  _stoppingDistance) 
                return false;
            
            _meleeEntityBehaviour.SetDirection(_meleeEntityBehaviour.Position.x > _target.transform.position.x ? Direction.Left : Direction.Right);
            ResetMovement();
            if(_searchCoroutine != null)
                ProjectUpdater.Instance.StopCoroutine(_searchCoroutine);
            _meleeEntityBehaviour.Land();
            _isAttacking = true;
            _meleeEntityBehaviour.StartAttack();
            return true;
        }
        
        private void ResetMovement()
        {
            _target = null;
            _currentPath = null;
            _meleeEntityBehaviour.MoveHorizontally(0);
        }

        private void OnAttack()
        {
            if (_meleeEntityBehaviour.TryGetAttackTarget(out BaseEntityBehaviour target))
                target.TakeDamage(StatValueGiver.GetValue(StatType.Damage));
        }

        private void OnAttackEnded()
        {
            _isAttacking = false;
            _searchCoroutine = ProjectUpdater.Instance.StartCoroutine(SearchCoroutine());
        }

        private void OnDisappeared()
        {
            _meleeEntityBehaviour.Disappeared -= OnDisappeared;

            if (_dropAmount <= 0) return;
            for (float i = 1; i <= _dropAmount; i++)
            {
                _dropGenerator.DropRandomItem(_meleeEntityBehaviour.transform.position + new Vector3(0,_meleeEntityBehaviour.Size.y));
            }
        }
    }
}