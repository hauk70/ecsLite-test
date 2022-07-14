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

            _fixedUpdateSystems.Add(new UpdateViewPositionSystem());
            _fixedUpdateSystems.Add(new PlayerInputSystem());
            _fixedUpdateSystems.Add(new MovementSystem());

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

                _world.GetPool<MovementSpeedComponent>()
                    .Add(entity)
                    .Speed = player.MoveSpeed;

                _world.GetPool<ViewComponent>()
                    .Add(entity)
                    .GameObject = player.gameObject;

                break;
            }

        }
    }
}