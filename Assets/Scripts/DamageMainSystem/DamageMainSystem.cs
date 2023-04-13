﻿using System.Linq;
using System.Runtime.CompilerServices;
using Config;
using Damage;
using DamageInfo;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Content;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

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

            var elapsedTime = (float) SystemAPI.Time.ElapsedTime;
            // var ecb = new EntityCommandBuffer(Allocator.Temp);
            // // 1. Add text glyphs to the entities who doesn't have them yet
            // foreach (var (request, localTransform, entity) 
            //          in SystemAPI.Query<RefRO<DamageRequest>, RefRO<LocalTransform>>().WithNone<DamageBubble>().WithEntityAccess().AsParallel())
            // {
            //     var bubble = ecb.Instantiate(config.DamageBubble);
            //     ecb.SetComponent(bubble, new LocalTransform()
            //     {
            //         Position = localTransform.ValueRO.Position, Rotation = quaternion.identity, Scale = 1
            //     });
            //     ecb.AddComponent(bubble, new DamageBubble() { SpawnTime = elapsedTime});
            // }
            // ecb.Playback(EntityManager);
            
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

            new ApplyGlyphsJob()
            {
                ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                ElapsedTime = elapsedTime,
                glyphEntity = config.DamageBubble
            }.ScheduleParallel();
        }
        
        
        [BurstCompile]
        [WithNone(typeof(DamageBubble))]
        public partial struct ApplyGlyphsJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter ecb;
            public Entity glyphEntity;
            public float ElapsedTime;            
            
            
            public void Execute([ChunkIndexInQuery] int chunkIndex, in DamageRequest damageRequest, in LocalTransform localTransform)
            {
                var request = damageRequest.Value;
                // split to numbers
                            
                
                
                var bubble = ecb.Instantiate(chunkIndex, glyphEntity);
                ecb.SetComponent(chunkIndex, bubble, new LocalTransform()
                {
                    Position = localTransform.Position, Rotation = quaternion.identity, Scale = 1
                });
                ecb.AddComponent(chunkIndex, bubble, new DamageBubble() { SpawnTime = ElapsedTime});
            }
        }
    }
}