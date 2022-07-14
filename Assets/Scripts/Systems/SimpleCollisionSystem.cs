using Leopotam.EcsLite;

namespace EcsTest
{
    public class SimpleCollisionSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();

            var playerFilter = world.Filter<TriggerActivatorComponent>()
                .Inc<PositionComponent>()
                .Inc<SizeComponent>()
                .End();

            var buttonsFilter = world.Filter<ButtonComponent>()
                .Inc<TriggerReactionsComponent>()
                .Inc<SizeComponent>()
                .Inc<PositionComponent>()
                .End();

            var positions = world.GetPool<PositionComponent>();
            var size = world.GetPool<SizeComponent>();
            var factPool = world.GetPool<TriggerFactComponent>();

            foreach (var playerEntity in playerFilter)
            {
                ref var playerPos = ref positions.Get(playerEntity);
                ref var playerSize = ref size.Get(playerEntity);

                foreach (var buttonEntity in buttonsFilter)
                {
                    ref var buttonPos = ref positions.Get(buttonEntity);
                    ref var buttonSize = ref size.Get(buttonEntity);

                    var sqrMagnitude = (buttonPos.Position - playerPos.Position).sqrMagnitude;
                    var range = playerSize.Radius + buttonSize.Radius;

                    if (!(sqrMagnitude < range * range))
                    {
                        factPool.Del(buttonEntity);
                    }
                    else if (!factPool.Has(buttonEntity))
                    {
                        ref var fact = ref factPool.Add(buttonEntity);
                        fact.Entity = buttonEntity;
                    }
                    else
                    {
                        ref var fact = ref factPool.Get(buttonEntity);
                        fact.Entity = buttonEntity;
                    }
                }
            }
        }
    }
}