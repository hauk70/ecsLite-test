using Leopotam.EcsLite;
using UnityEngine;

namespace EcsTest
{
    public class PlayerInputSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();

            Camera camera = null;
            var sceneSettingsFilter = world.Filter<CameraComponent>()
                .End();
            var settingsPool = world.GetPool<CameraComponent>();
            foreach (var entity in sceneSettingsFilter)
            {
                ref var sceneSettingsComponent = ref settingsPool.Get(entity);
                camera = sceneSettingsComponent.Camera;
                break;
            }

            if (!Input.GetMouseButton(0))
                return;

            var ray = camera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit, 1000
                //, LayerMask.NameToLayer("Ground")
            ))
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