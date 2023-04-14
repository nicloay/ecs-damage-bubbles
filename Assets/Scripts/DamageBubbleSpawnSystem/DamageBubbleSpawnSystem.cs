using Config;
using Damage;
using DamageInfo;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace DamageProxySystem
{
    /// <summary>
    ///     Replace DamageRequest tag with DamageBubble text
    /// </summary>
    public partial struct DamageBubbleSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<ConfigData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var config = SystemAPI.GetSingleton<ConfigData>();
            var elapsedTime = (float)SystemAPI.Time.ElapsedTime;
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

            new ApplyGlyphsJob
            {
                Ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                ElapsedTime = elapsedTime,
                GlyphEntity = config.GlyphPrefab,
                GlyphZOffset = config.GlyphZOffset,
                GlyphWidth = config.GlyphWidth
            }.ScheduleParallel();
        }


        [BurstCompile]
        [WithNone(typeof(DamageBubble))]
        public partial struct ApplyGlyphsJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter Ecb;
            public Entity GlyphEntity;
            public float ElapsedTime;
            public float GlyphZOffset;
            public float GlyphWidth;

            public void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity, in LocalTransform transform,
                in DamageRequest damageRequest)
            {
                var number = damageRequest.Value;
                var glyphTransform = transform;
                if (number > 9)
                {
                    var offset = math.log10(number) / 2f * GlyphWidth;
                    glyphTransform.Position.x += offset;
                }
                else
                {
                    Debug.Log(number);
                }

                // split to numbers
                // we iterate from  rightmost digit to leftmost
                while (number > 0)
                {
                    var digit = number % 10;
                    number /= 10;
                    var glyph = Ecb.Instantiate(chunkIndex, GlyphEntity);
                    Ecb.SetComponent(chunkIndex, glyph, new GlyphIdFloatOverride { Value = digit });
                    Ecb.SetComponent(chunkIndex, glyph, glyphTransform);
                    glyphTransform.Position.x -= GlyphWidth;
                    glyphTransform.Position.z -= GlyphZOffset;
                    Ecb.AddComponent(chunkIndex, glyph,
                        new DamageBubble { SpawnTime = ElapsedTime, OriginalY = glyphTransform.Position.y });
                }

                Ecb.DestroyEntity(chunkIndex, entity);
            }
        }
    }
}