using System;
using Leopotam.EcsLite;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EcsTest
{
    public class EcsController : IDisposable
    {
        private readonly EcsWorld _world;
        private readonly EcsSystems _updateSystems;
        private readonly EcsSystems _fixedUpdateSystems;

        public EcsController()
        {
            _world = new EcsWorld();

            _updateSystems = new EcsSystems(_world);
            _fixedUpdateSystems = new EcsSystems(_world);

            _fixedUpdateSystems.Add(new PlayerInputSystem());
            _fixedUpdateSystems.Add(new MovementSystem());
            _fixedUpdateSystems.Add(new UpdateViewPositionSystem());
            _fixedUpdateSystems.Add(new SimpleCollisionSystem());

            MapEntities();

            _updateSystems.Init();
            _fixedUpdateSystems.Init();
        }

        public void Update()
        {
            _updateSystems?.Run();
        }

        public void FixedUpdate()
        {
            _fixedUpdateSystems?.Run();
        }

        public void Dispose()
        {
            _world?.Destroy();
            _updateSystems?.Destroy();
            _fixedUpdateSystems?.Destroy();
        }

        void MapEntities()
        {
            var time = _world.NewEntity();
            _world.GetPool<TimeComponent>()
                .Add(time)
                .delta = Time.fixedDeltaTime;

            var camera = _world.NewEntity();
            _world.GetPool<CameraComponent>()
                .Add(camera)
                .Camera = Camera.main;

            foreach (var player in Object.FindObjectsOfType<PlayerView>())
            {
                var entity = _world.NewEntity();

                _world.GetPool<PositionComponent>()
                    .Add(entity)
                    .Position = player.transform.position;

                _world.GetPool<SizeComponent>()
                    .Add(entity)
                    .Radius = player.BodyRadius;

                _world.GetPool<TriggerActivatorComponent>()
                    .Add(entity);

                _world.GetPool<MovementSpeedComponent>()
                    .Add(entity)
                    .Speed = player.MoveSpeed;

                _world.GetPool<ViewComponent>()
                    .Add(entity)
                    .GameObject = player.gameObject;

                break;
            }

            foreach (var button in Object.FindObjectsOfType<ButtonView>())
            {
                var entity = _world.NewEntity();

                _world.GetPool<ButtonComponent>()
                    .Add(entity)
                    .Id = button.Id;

                _world.GetPool<SizeComponent>()
                    .Add(entity)
                    .Radius = button.TriggerRadius;

                _world.GetPool<PositionComponent>()
                    .Add(entity)
                    .Position = button.transform.position;

                _world.GetPool<TriggerReactionsComponent>()
                    .Add(entity);
            }

            foreach (var door in Object.FindObjectsOfType<DoorView>())
            {
                var entity = _world.NewEntity();

                _world.GetPool<PositionComponent>()
                    .Add(entity)
                    .Position = door.transform.position;

                _world.GetPool<TriggerActivatorComponent>()
                    .Add(entity);

                _world.GetPool<ViewComponent>()
                    .Add(entity)
                    .GameObject = door.gameObject;
            }
        }
    }
}