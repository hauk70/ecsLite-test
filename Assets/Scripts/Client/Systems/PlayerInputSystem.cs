using EcsTest.Client.Components;
using EcsTest.Shared.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace EcsTest.Client.Systems
{
    public class PlayerInputSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();

            Camera camera = Utils.FetchFirst<CameraComponent>(world).Camera;

            if (!Input.GetMouseButton(0))
                return;

            var ray = camera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit, 1000,LayerMask.GetMask("Ground")))
                return;

            var goToCommandFilter = world.Filter<GoToComponent>()
                .End();
            var goToPool = world.GetPool<GoToComponent>();

            if (goToCommandFilter.GetEntitiesCount() == 0)
            {
                var entity = world.NewEntity();
                SetTarget(ref goToPool.Add(entity), hit.point);
            }
            else
            {
                foreach (var entity in goToCommandFilter)
                {
                    SetTarget(ref goToPool.Get(entity), hit.point);
                    break;
                }
            }
        }

        private void SetTarget(ref GoToComponent component, Vector3 target)
        {
            component.Target = target;
        }
    }
}