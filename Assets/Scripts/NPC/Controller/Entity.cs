using System;
using Core.Parallax;
using NPC.Behaviour;
using StatsSystem.Data;
using StatsSystem.Enums;
using UnityEngine;

namespace NPC.Controller
{
    public abstract class Entity : IDisposable
    {
        private readonly BaseEntityBehaviour _entityBehaviour;
        protected readonly IStatValueGiver StatValueGiver;
        protected readonly IParallaxTargetMovement ParallaxPlayerMovement;

        private float _currentHP;

        public event Action<Entity> Died;
        

        protected Entity(BaseEntityBehaviour entityBehaviour, IStatValueGiver statValueGiver, IParallaxTargetMovement parallaxPlayerMovement)
        {
            _entityBehaviour = entityBehaviour;
            _entityBehaviour.Initialize();
            StatValueGiver = statValueGiver;
            ParallaxPlayerMovement = parallaxPlayerMovement;
            _currentHP = StatValueGiver.GetValue(StatType.Health);
            _entityBehaviour.DamageTaken += OnDamageTaken;
            Died += OnDeath;
        }

        public virtual void Dispose()
        {
            _entityBehaviour.DamageTaken -= OnDamageTaken;
            Died -= OnDeath;
        }
        
        protected abstract void VisualiseHP(float currentHP);
        
        private void OnDeath(Entity entity)
        {
            entity.Dispose();
            _entityBehaviour.Die();
        }

        private void OnDamageTaken(float damage)
        {
            damage -= StatValueGiver.GetValue(StatType.Defence);
            if (damage <= 0)
                return;

            _currentHP -= damage;
           
            VisualiseHP(_currentHP);

            if (_currentHP <= 0)
            {
                Died?.Invoke(this);
                Debug.Log($"Killed {this}");
            }
        }
    }
}