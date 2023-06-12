using System;
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
        private const float CoroutineCycleTime = 0.2f;
        
        private readonly Seeker _seeker;
        private readonly MeleeEntityBehaviour _meleeEntityBehaviour;
        private readonly Vector2 _moveDelta;
        private readonly DropGenerator _dropGenerator;
        private readonly int _dropAmount;
        
        private bool _isAttacking;
        private Coroutine _searchCoroutine;
        private Collider2D _target;
        private Vector3 _previousTargetPosition;
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
            var speedDelta = StatValueGiver.GetValue(StatType.Speed) * ParallaxPlayerMovement.ParallaxSpeedCoef * Time.fixedDeltaTime;
            _moveDelta = new Vector2(speedDelta, speedDelta);
            
            VisualiseHP(StatValueGiver.GetValue(StatType.Health));
            _meleeEntityBehaviour.AttackImpacted += OnAttack;
            _meleeEntityBehaviour.Disappeared += OnDisappeared;
        }

        public override void Dispose()
        {
            base.Dispose();
            _meleeEntityBehaviour.AttackSequenceEnded -= OnAttackEnded;
            ProjectUpdater.Instance.FixedUpdateCalled -= OnFixedUpdateCalled;
            if(_searchCoroutine != null)
                ProjectUpdater.Instance.StopCoroutine(_searchCoroutine);
            
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
                else if(_target.transform.position != _previousTargetPosition)
                {
                    Vector2 position = _target.transform.position;
                    _previousTargetPosition = position;
                    _stoppingDistance = (_target.bounds.size.x + _meleeEntityBehaviour.Size.x) / 2;
                    var delta = position.x < _meleeEntityBehaviour.transform.position.x ? 1 : -1;
                    _destination = position + new Vector2(_stoppingDistance * delta, 0);
                    _seeker.StartPath(_meleeEntityBehaviour.transform.position, _destination, OnPathCalculated);
                }
                yield return new WaitForSeconds(CoroutineCycleTime);
            }
        }
        
        private bool TryGetTarget(out Collider2D target)
        {
            target = Physics2D.OverlapBox(_meleeEntityBehaviour.transform.position, _meleeEntityBehaviour.SearchBox, 0,
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
            if (_isAttacking || _target == null || _currentPath == null || CheckIfCanAttack() || _currentWayPoint >= _currentPath.vectorPath.Count) 
                return;
            
            var currentPosition = _meleeEntityBehaviour.transform.position;
            var waypointPosition = _currentPath.vectorPath[_currentWayPoint];
            var waypointDirection = waypointPosition - currentPosition;

            if (Vector2.Distance(waypointPosition, currentPosition) < 0.05f)
            {
                _currentWayPoint++;
                return;
            }
            
            if (waypointDirection.y != 0)
            {
                waypointDirection.y = waypointDirection.y > 0 ? 1 : -1;
                var newVerticalPosition = currentPosition.y + _moveDelta.y * waypointDirection.y;
                if (waypointDirection.y > 0 && waypointPosition.y < newVerticalPosition ||
                    waypointDirection.y < 0 && waypointPosition.y > newVerticalPosition)
                    newVerticalPosition = waypointPosition.y;
                
                if (Math.Abs(newVerticalPosition - _meleeEntityBehaviour.transform.position.y) > Mathf.Epsilon)
                    _meleeEntityBehaviour.MoveVertically(newVerticalPosition);
            }

            if (waypointDirection.x == 0) 
                return;
            
            waypointDirection.x = waypointDirection.x > 0 ? 1 : -1;
            var newHorizontalPosition = currentPosition.x + _moveDelta.x * waypointDirection.x;
            if (waypointDirection.x > 0 && waypointPosition.x < newHorizontalPosition ||
                waypointDirection.x < 0 && waypointPosition.x > newHorizontalPosition)
                newHorizontalPosition = waypointPosition.x;
                
            if(Math.Abs(newHorizontalPosition - _meleeEntityBehaviour.transform.position.x) > Mathf.Epsilon)
                _meleeEntityBehaviour.MoveHorizontally(newHorizontalPosition);
        }

        private bool CheckIfCanAttack()
        {
            var distance = _destination - _meleeEntityBehaviour.transform.position;
            if (Mathf.Abs(distance.x) > 0.2f || Mathf.Abs(distance.y) > 0.2f) 
                return false;
            
            _meleeEntityBehaviour.MoveHorizontally(_destination.x);
            _meleeEntityBehaviour.MoveVertically(_destination.y);
            _meleeEntityBehaviour.SetDirection( _target.transform.position.x < _meleeEntityBehaviour.transform.position.x ? Direction.Left : Direction.Right);
            ResetMovement();
            if(_searchCoroutine != null)
                ProjectUpdater.Instance.StopCoroutine(_searchCoroutine);
            _isAttacking = true;
            _meleeEntityBehaviour.StartAttack();

            return true;
        }
        
        private void ResetMovement()
        {
            _target = null;
            _currentPath = null;
            _previousTargetPosition = Vector2.negativeInfinity;
            var position = _meleeEntityBehaviour.transform.position;
            _meleeEntityBehaviour.MoveHorizontally(position.x);
            _meleeEntityBehaviour.MoveVertically(position.y);
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
                _dropGenerator.DropRandomItem(_meleeEntityBehaviour.transform.position);
            }
        }
    }
}