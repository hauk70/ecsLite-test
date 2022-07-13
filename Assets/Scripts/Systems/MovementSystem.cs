using Leopotam.EcsLite;

namespace EcsTest
{
    public class MovementSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();

            var entities = world.Filter<MovementSpeedComponent>()
                .Inc<PositionComponent>()
                .End();

            var movementSpeedPool = world.GetPool<MovementSpeedComponent>();
            var positionsPool = world.GetPool<PositionComponent>();

            var targetFilter = world.Filter<GoToComponent>()
                .End();
            GoToComponent target = default;
            foreach (var targetEntity in targetFilter)
            {
                target = world.GetPool<GoToComponent>().Get(targetEntity);
                break;
            }

            foreach (var entity in entities)
            {
                ref var speed = ref movementSpeedPool.Get(entity);
                ref var position = ref positionsPool.Get(entity);

                position.Position = target.Target;
            }
        }
    }
}