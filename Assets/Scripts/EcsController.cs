using System;
using Leopotam.EcsLite;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EcsTest
{
    public class EcsController : IDisposable
    {
        private readonly EcsWorld _world;
        private readonly EcsSystems _fixedUpdateSystems;

        public EcsController()
        {
            _world = new EcsWorld();

            _fixedUpdateSystems = new EcsSystems(_world);

            _fixedUpdateSystems.Add(new PlayerInputSystem());
            _fixedUpdateSystems.Add(new MovementSystem());
            _fixedUpdateSystems.Add(new UpdateViewPositionSystem());
            _fixedUpdateSystems.Add(new PlayerAnimationSystem());
            _fixedUpdateSystems.Add(new SimpleCollisionSystem());
            _fixedUpdateSystems.Add(new DoorActivationSystem());

            PrepareScene();
            CreateEntitiesFromLevel();

            _fixedUpdateSystems.Init();
        }

        public void FixedUpdate()
        {
            _fixedUpdateSystems?.Run();
        }

        public void Dispose()
        {
            _world?.Destroy();
            _fixedUpdateSystems?.Destroy();
        }

        void PrepareScene()
        {
            var time = _world.NewEntity();
            _world.GetPool<TimeComponent>()
                .Add(time)
                .delta = Time.fixedDeltaTime;

            var camera = _world.NewEntity();
            _world.GetPool<CameraComponent>()
                .Add(camera)
                .Camera = Camera.main;
        }

        void CreateEntitiesFromLevel()
        {
            foreach (var player in Object.FindObjectsOfType<PlayerView>())
            {
                var entity = _world.NewEntity();

                CreatePositionComponent(entity, player);
                CreateSizeComponent(entity, player.BodyRadius);
                CreateViewComponent(entity, player.gameObject);

                _world.GetPool<AnimatorComponent>()
                    .Add(entity)
                    .Animator = player.Animator;

                _world.GetPool<TriggerActivatorComponent>()
                    .Add(entity);

                _world.GetPool<MovementSpeedComponent>()
                    .Add(entity)
                    .Speed = player.MoveSpeed;

                break;
            }

            foreach (var button in Object.FindObjectsOfType<ButtonView>())
            {
                var entity = _world.NewEntity();

                CreatePositionComponent(entity, button);
                CreateSizeComponent(entity, button.TriggerRadius);
                CreateViewComponent(entity, button.gameObject);

                _world.GetPool<ButtonComponent>()
                    .Add(entity)
                    .Id = button.Id;

                _world.GetPool<TriggerReactionsComponent>()
                    .Add(entity);
            }

            foreach (var door in Object.FindObjectsOfType<DoorView>())
            {
                var entity = _world.NewEntity();

                CreatePositionComponent(entity, door);
                CreateViewComponent(entity, door.gameObject);

                _world.GetPool<ProgressComponent>()
                    .Add(entity);

                ref var doorComponent = ref _world.GetPool<DoorComponent>()
                    .Add(entity);
                doorComponent.TriggerId = door.TriggerId;
                doorComponent.OpenedPosition = door.OpenPosition;
                doorComponent.ClosedPosition = door.ClosedPosition;
                doorComponent.OpeningTime = door.OpeningTime;
            }
        }

        private void CreateViewComponent(int entity, GameObject obj)
        {
            _world.GetPool<ViewComponent>()
                .Add(entity)
                .GameObject = obj;
        }

        private void CreatePositionComponent(int entity, MonoBehaviour obj)
        {
            _world.GetPool<PositionComponent>()
                .Add(entity)
                .Position = obj.transform.position;
        }

        private void CreateSizeComponent(int entity, float radius)
        {
            _world.GetPool<SizeComponent>()
                .Add(entity)
                .Radius = radius;
        }
    }
}