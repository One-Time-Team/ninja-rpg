using Core.Enums;
using StatsSystem;
using UnityEngine;

namespace Core.Movement.Controllers
{
    public class Mover
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly Transform _transform;
        private readonly IStatValueGiver _statValueGiver;

        private float _direction;
        
        public Direction FaceDirection { get; private set; } = Direction.Right;
        public bool IsMoving => _direction != 0;
        
        
        public Mover(Rigidbody2D rigidbody, IStatValueGiver statValueGiver)
        {
            _rigidbody = rigidbody;
            _transform = rigidbody.transform;
            _statValueGiver = statValueGiver;
        }
        
        public void MoveHorizontally(float direction)
        {
            _direction = direction;
            SetDirection(direction);
            Vector2 velocity = _rigidbody.velocity;
            velocity.x = direction * _statValueGiver.GetValue(StatType.Speed) * Time.deltaTime;
            _rigidbody.velocity = velocity;
        }
        
        private void SetDirection(float direction)
        {
            if ((FaceDirection == Direction.Right && direction < 0) ||
                (FaceDirection == Direction.Left && direction > 0))
            {
                FlipSide();
            }
        }

        private void FlipSide()
        {
            _transform.Rotate(0, 180, 0);
            FaceDirection = FaceDirection == Direction.Right ? Direction.Left : Direction.Right;
        }
    }
}