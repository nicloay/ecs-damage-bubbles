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
    ///     Contains object pool for every glyph,
    ///     when DamageTag appeared, it attach glyphs and damage bubble to the entity
    ///     so it will start going up with proper text
    /// </summary>
    public partial struct DamageMainSystem : ISystem
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

            new ApplyGlyphsJob()
            {
                Ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                ElapsedTime = elapsedTime,
                GlyphEntity = config.DamageBubble
            }.ScheduleParallel();
        }


        [BurstCompile]
        [WithNone(typeof(DamageBubble))]
        public partial struct ApplyGlyphsJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter Ecb;
            public Entity GlyphEntity;
            public float ElapsedTime;
            private const float GLYPH_WIDTH = 1f;

            public void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity, in LocalTransform transform, in DamageRequest damageRequest)
            {
                var number = damageRequest.Value;
                // split to numbers
                var offset = math.log10(number) / 2f * GLYPH_WIDTH;
                var t = transform;
                t.Position.x += offset;
                
                // we iterate from  rightmost digit to leftmost
                while (number > 0)
                {
                    var digit = number % 10;
                    number /= 10;


                    var glyph = Ecb.Instantiate(chunkIndex, GlyphEntity);
                    
                    //Ecb.AddComponent(chunkIndex, glyph, new Parent(){ Value = entity});
                    Ecb.SetComponent(chunkIndex, glyph, new GlyphIdFloatOverride(){Value = digit});
                    Ecb.SetComponent(chunkIndex, glyph, t);
                    t.Position.x -= GLYPH_WIDTH;
                    Ecb.AddComponent(chunkIndex, glyph, new DamageBubble() { SpawnTime = ElapsedTime });
                    offset-= GLYPH_WIDTH;
                }
                
                Ecb.DestroyEntity(chunkIndex, entity);
            }
        }
    }
}