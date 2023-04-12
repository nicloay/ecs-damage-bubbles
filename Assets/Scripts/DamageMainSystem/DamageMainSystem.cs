using System.Runtime.CompilerServices;
using Config;
using Damage;
using DamageInfo;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Content;
using Unity.Mathematics;
using Unity.Transforms;

namespace DamageProxySystem
{
    /// <summary>
    ///     Contains object pool for every glyph,
    ///     when DamageTag appeared, it attach glyphs and damage bubble to the entity
    ///     so it will start going up with proper text
    /// </summary>
    public partial class DamageMainSystem : SystemBase
    {
     
     
        protected override void OnUpdate()
        {
            var config = SystemAPI.GetSingleton<ConfigData>();

            var ecb = new EntityCommandBuffer(Allocator.Temp);
            // 1. Add text glyphs to the entities who doesn't have them yet
            foreach (var (request, localTransform, entity) 
                     in SystemAPI.Query<RefRO<DamageRequest>, RefRO<LocalTransform>>().WithNone<DamageBubble>().WithEntityAccess())
            {
                var bubble = ecb.Instantiate(config.DamageBubble);
                ecb.SetComponent(bubble, new LocalTransform()
                {
                    Position = localTransform.ValueRO.Position, Rotation = quaternion.identity, Scale = 1
                });
                ecb.AddComponent(bubble, new DamageBubble());
            }
            ecb.Playback(EntityManager);
            
            // 2. destroy entities which has specific tag
        }
    }
}