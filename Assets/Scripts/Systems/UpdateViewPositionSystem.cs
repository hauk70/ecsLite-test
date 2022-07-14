using Leopotam.EcsLite;
using EcsTest.Components;

namespace EcsTest.Systems
{
    public class UpdateViewPositionSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();

            var movableObjectsFilter = world.Filter<PositionComponent>()
                .Inc<ViewComponent>()
                .End();

            var viewPool = world.GetPool<ViewComponent>();
            var positionsPool = world.GetPool<PositionComponent>();

            foreach (int entity in movableObjectsFilter)
            {
                var position = positionsPool.Get(entity);
                var view = viewPool.Get(entity);

                view.GameObject.transform.position = position.Position;
            }
        }
    }
}