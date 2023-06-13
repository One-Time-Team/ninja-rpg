using Core.Enums;
using UnityEngine;

namespace Core.Movement.Controllers
{
    public abstract class Mover
    {
        protected readonly Rigidbody2D Rigidbody;

        public Direction FaceDirection { get; private set; }
        public abstract bool IsMoving { get; }
        
        
        protected Mover(Rigidbody2D rigidbody)
        {
            Rigidbody = rigidbody;
            FaceDirection = Direction.Right;
        }

        public abstract void MoveHorizontally(float movement);

        public void SetDirection(Direction newDirection)
        {
            if (FaceDirection == newDirection) 
                return;
            
            Rigidbody.transform.Rotate(0, 180, 0);
            FaceDirection = FaceDirection == Direction.Right ? Direction.Left : Direction.Right;
        }
    }
}