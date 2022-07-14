using EcsTest.Components;
using EcsTest.Components.Client;
using Leopotam.EcsLite;
using UnityEngine;

namespace EcsTest.Systems.Client
{
    public class PlayerAnimationSystem : IEcsRunSystem
    {
        private static readonly int MoveParameter = Animator.StringToHash("Move");

        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();

            var players = world.Filter<AnimatorComponent>()
                .End();
            var goToFilter = world.Filter<GoToComponent>()
                .End();
            
            foreach (var playerEntity in players)
            {
                var playerAnimator = world.GetPool<AnimatorComponent>().Get(playerEntity);

                playerAnimator.Animator.SetBool(MoveParameter, goToFilter.GetEntitiesCount() > 0);

                break;
            }
        }
    }
}