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
        protected override void OnUpdate()
        {
            float3 min = new float3(-15, 0, -15);
            float3 max = new float3(15, 0, 15);
            var rnd = new Random((uint)SystemAPI.Time.GetHashCode());
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var transform in SystemAPI.Query<RefRO<LocalTransform>>().WithAny<TestSpawner>())
            {
                var entity = ecb.CreateEntity();
                ecb.AddComponent(entity, new DamageRequest { Value = rnd.NextInt(1, 999999) });

                ecb.AddComponent(entity, new LocalTransform
                {
                    Scale = 1, Position = rnd.NextFloat3(min, max), Rotation = Quaternion.identity
                });
            }

            ecb.Playback(EntityManager);
        }
    }
}