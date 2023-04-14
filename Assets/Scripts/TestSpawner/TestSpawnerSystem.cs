using Damage;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace TestSpawner
{
    public partial class TestSpawnerSystem : SystemBase
    {
        private Random _rnd;

        protected override void OnCreate()
        {
            _rnd = new Random((uint)SystemAPI.Time.GetHashCode());
        }

        protected override void OnUpdate()
        {
            var min = new float3(-15, 0, -5);
            var max = new float3(15, 0, 5);

            //if ((int)(SystemAPI.Time.ElapsedTime) % 3 != 0) return;

            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var transform in SystemAPI.Query<RefRO<LocalTransform>>().WithAny<TestSpawner>())
            {
                var entity = ecb.CreateEntity();

                ecb.AddComponent(entity, new DamageRequest { Value = _rnd.NextInt(1, 9) });

                ecb.AddComponent(entity, new LocalTransform
                {
                    Scale = 1, Position = _rnd.NextFloat3(min, max), Rotation = Quaternion.identity
                });
            }

            ecb.Playback(EntityManager);
        }
    }
}