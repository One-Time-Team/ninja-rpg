using UnityEngine;

namespace Core.Movement.Controllers
{
    public class Jumper
    {
        private readonly Rigidbody2D _rigidbody;

        public bool IsJumping { get; private set; }
        public bool IsLanding { get; private set; }


        public Jumper(Rigidbody2D rigidbody)
        {
            _rigidbody = rigidbody;
        }
        
        public bool StartJump(float jumpForce)
        {
            if (IsJumping || IsLanding)
                return false;

            IsJumping = true;
            _rigidbody.AddForce(Vector2.up * jumpForce);
            
            return true;
        }

        public bool GetOnGround(Collision2D ground)
        {
            if (ground.transform.CompareTag("Ground"))
            {
                IsJumping = false;
                IsLanding = false;
                return true;
            }
            return false;
        }

        public bool GetOffGround(Collision2D ground)
        {
            if (ground.transform.CompareTag("Ground"))
            {
                return true;
            }
            return false;
        }

        public void StartLanding() => IsLanding = true;
    }
}