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
            Land();
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
            if (!ground.transform.CompareTag("Ground")) return false;
            Land();
            return true;
        }

        public bool GetOffGround(Collision2D ground)
        {
            return ground.transform.CompareTag("Ground");
        }

        public void StartLanding()
        {
            IsLanding = true;
            IsJumping = false;
        }
        
        public void Land()
        {
            IsLanding = false;
            IsJumping = false;
        }
    }
}