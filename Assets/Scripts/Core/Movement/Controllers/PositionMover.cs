using System;
using Core.Enums;
using UnityEngine;

namespace Core.Movement.Controllers
{
    public class PositionMover : Mover
    {
        private Vector2 _destination;
        
        public override bool IsMoving => _destination != Rigidbody.position;
        
        
        public PositionMover(Rigidbody2D rigidbody) : base(rigidbody) { }

        public override void MoveHorizontally(float horizontalDestination)
        {
            _destination.x = horizontalDestination;
            var newPosition = new Vector2(horizontalDestination, Rigidbody.position.y);
            Rigidbody.MovePosition(newPosition);

            if (horizontalDestination == Rigidbody.position.x) return;
            SetDirection(Rigidbody.position.x < horizontalDestination ? Direction.Right : Direction.Left);
        }
        
        public override void MoveVertically(float verticalDestination)
        {
            _destination.y = verticalDestination;
            var newPosition = new Vector2(Rigidbody.position.x, verticalDestination);
            Rigidbody.MovePosition(newPosition);
        }
    }
}