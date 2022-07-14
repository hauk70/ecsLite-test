using EcsTest.Components;
using Leopotam.EcsLite;

namespace EcsTest.Systems
{
    public class MovementSystem : IEcsRunSystem
    {
        private const float MoveError = .05f;

        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();

            var entities = world.Filter<MovementSpeedComponent>()
                .Inc<PositionComponent>()
                .End();

            var movementSpeedPool = world.GetPool<MovementSpeedComponent>();
            var positionsPool = world.GetPool<PositionComponent>();

            var goToFilter = world.Filter<GoToComponent>()
                .End();

            if (goToFilter.GetEntitiesCount() == 0)
                return;
            
            var time = Utils.FetchFirst<TimeComponent>(world);

            var goToPool = world.GetPool<GoToComponent>();
            GoToComponent targetPos = default;
            int goToEntity = default;
            foreach (var targetEntity in goToFilter)
            {
                targetPos = goToPool.Get(targetEntity);
                goToEntity = targetEntity;
                break;
            }

            foreach (var entity in entities)
            {
                ref var speed = ref movementSpeedPool.Get(entity);
                ref var position = ref positionsPool.Get(entity);

                var newPos = position.Position +
                             (targetPos.Target - position.Position).normalized * speed.Speed * time.delta;

                if ((newPos - targetPos.Target).sqrMagnitude > MoveError)
                    position.Position = newPos;
                else
                {
                    goToPool.Del(goToEntity);
                }
            }
        }
    }
}