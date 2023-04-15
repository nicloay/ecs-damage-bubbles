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
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var spawner in SystemAPI.Query<TestSpawner>())
            {
                var entities = spawner.EntitiesPerFrame;
                while (entities-- > 0)
                {
                    var entity = ecb.CreateEntity();
                    ecb.AddComponent(entity, new DamageRequest { Value = _rnd.NextInt(1, 999999) });

                    ecb.AddComponent(entity, new LocalTransform
                    {
                        Scale = 1, Position = _rnd.NextFloat3(spawner.MinPosition, spawner.MaxPosition), Rotation = Quaternion.identity
                    });
                }
                
            }

            ecb.Playback(EntityManager);
        }
    }
}