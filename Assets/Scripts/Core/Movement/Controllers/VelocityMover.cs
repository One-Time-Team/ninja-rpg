using Core.Enums;
using UnityEngine;

namespace Core.Movement.Controllers
{
    public class VelocityMover : Mover
    {
        private float _direction;
        
        public override bool IsMoving => _direction != 0;
        
        
        public VelocityMover(Rigidbody2D rigidbody) : base(rigidbody){}
        
        public override void MoveHorizontally(float direction)
        {
            _direction = direction;
            Vector2 velocity = Rigidbody.velocity;
            velocity.x = direction * Time.deltaTime;
            Rigidbody.velocity = velocity;
            
            if (!IsMoving) return;
            SetDirection(direction > 0 ? Direction.Right : Direction.Left);
        }
    }
}