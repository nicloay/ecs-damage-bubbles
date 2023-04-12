using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DamageInfo
{
    public partial class DamageBubbleMovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (bubble, transform, entity)
                     in SystemAPI.Query<RefRW<DamageBubble>, RefRW<LocalTransform>>().WithEntityAccess())
            {
                transform.ValueRW.Position += SystemAPI.Time.DeltaTime * math.up();
                bubble.ValueRW.LifeTime += SystemAPI.Time.DeltaTime; // TODO: move this to singleton config
                if (bubble.ValueRO.LifeTime > 1.5f) ecb.DestroyEntity(entity);
            }
            ecb.Playback(EntityManager);
        }
    }
}