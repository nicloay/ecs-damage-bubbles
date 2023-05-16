using EcsDamageBubbles.Config;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace EcsDamageBubbles.Demo
{
    public partial class TestSpawnerSystem : SystemBase
    {
        private Random _rnd;
        
        protected override void OnCreate()
        {
            _rnd = new Random((uint)SystemAPI.Time.GetHashCode());
            RequireForUpdate<DamageBubbleColorConfig>();
        }

        protected override void OnUpdate()
        {
            var colorConfig = SystemAPI.GetSingletonBuffer<DamageBubbleColorConfig>();
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var spawner in SystemAPI.Query<TestSpawner>())
            {
                var entities = spawner.EntitiesPerFrame;
                while (entities-- > 0)
                {
                    var colorRND = _rnd.NextInt(0, 10);
                    var colorId = 0;
                    if (colorRND > 6 )
                    {
                        colorId = colorRND > 8 ? 1 : 2;
                    }
                    
                    var entity = ecb.CreateEntity();
                    ecb.AddComponent(entity, new DamageBubbleRequest
                    {
                        Value = _rnd.NextInt(1, 999999),
                        ColorId = colorId
                    });

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