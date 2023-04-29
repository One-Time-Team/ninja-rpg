using Core.Enums;
using StatsSystem;
using UnityEngine;

namespace Core.Movement.Controllers
{
    public class Jumper
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly IStatValueGiver _statValueGiver;
        
        public bool IsJumping { get; private set; }
        public bool IsLanding { get; private set; }


        public Jumper(Rigidbody2D rigidbody, IStatValueGiver statValueGiver)
        {
            _rigidbody = rigidbody;
            _statValueGiver = statValueGiver;
        }
        
        public bool StartJump()
        {
            if (IsJumping || IsLanding)
                return false;

            IsJumping = true;
            
            _rigidbody.AddForce(Vector2.up * _statValueGiver.GetValue(StatType.JumpForce));
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