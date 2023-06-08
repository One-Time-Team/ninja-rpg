using System;
using Core.Parallax;
using NPC.Behaviour;
using StatsSystem.Data;

namespace NPC.Controller
{
    public abstract class Entity
    {
        private readonly BaseEntityBehaviour _entityBehaviour;
        protected readonly IStatValueGiver StatValueGiver;
        protected readonly IParallaxTargetMovement ParallaxPlayerMovement;

        public event Action<Entity> Died;
        

        protected Entity(BaseEntityBehaviour entityBehaviour, IStatValueGiver statValueGiver, IParallaxTargetMovement parallaxPlayerMovement)
        {
            _entityBehaviour = entityBehaviour;
            _entityBehaviour.Initialize();
            StatValueGiver = statValueGiver;
            ParallaxPlayerMovement = parallaxPlayerMovement;
        }
    }
}