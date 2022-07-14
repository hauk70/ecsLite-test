using EcsTest.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace EcsTest.Systems
{
    public class DoorActivationSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();

            var doors = world.Filter<PositionComponent>()
                .Inc<ProgressComponent>()
                .Inc<DoorComponent>()
                .End();

            var buttons = world.Filter<ButtonComponent>()
                .End();

            var positions = world.GetPool<PositionComponent>();
            var doorsData = world.GetPool<DoorComponent>();
            var progressPool = world.GetPool<ProgressComponent>();
            var buttonsPool = world.GetPool<ButtonComponent>();
            var triggerFacts = world.GetPool<TriggerFactComponent>();
            var time = Utils.FetchFirst<TimeComponent>(world);

            foreach (var door in doors)
            {
                ref var doorProgress = ref progressPool.Get(door);
                if (doorProgress.Progress >= 1f)
                    continue;

                var doorData = doorsData.Get(door);

                if (!GetButtonEntityById(buttons, buttonsPool, doorData.TriggerId, out var buttonEntity))
                    continue;

                if (!triggerFacts.Has(buttonEntity))
                    continue;

                doorProgress.Progress = Mathf.Clamp01(doorProgress.Progress + time.delta / doorData.OpeningTime);

                ref var position = ref positions.Get(door);
                position.Position =
                    Vector3.Lerp(doorData.ClosedPosition, doorData.OpenedPosition, doorProgress.Progress);
            }
        }

        private bool GetButtonEntityById(EcsFilter buttons, EcsPool<ButtonComponent> pool, uint triggerId,
            out int buttonEntity)
        {
            foreach (var entity in buttons)
            {
                if (pool.Get(entity).Id == triggerId)
                {
                    buttonEntity = entity;
                    return true;
                }
            }

            buttonEntity = default;
            return false;
        }
    }
}