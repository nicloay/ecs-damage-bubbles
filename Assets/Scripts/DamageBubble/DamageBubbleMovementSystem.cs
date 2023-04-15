using System.Runtime.CompilerServices;
using Config;
using DamageProxySystem;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace DamageInfo
{
    [UpdateAfter(typeof(DamageBubbleSpawnSystem))]
    public partial struct DamageBubbleMovementSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<ConfigData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var config = SystemAPI.GetSingleton<ConfigData>();
            new MoveJob
            {
                ElapsedTime = (float)SystemAPI.Time.ElapsedTime,
                DeltaTime = SystemAPI.Time.DeltaTime,
                ECBWriter = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                lifeTime = config.MovementTime,
                VerticalOffset = config.VerticalOffset,
                ScaleOffset = config.ScaleOffset
            }.ScheduleParallel();
        }

        [BurstCompile]
        public partial struct MoveJob : IJobEntity
        {
            public float ElapsedTime;
            public float DeltaTime;
            public EntityCommandBuffer.ParallelWriter ECBWriter;
            public float lifeTime;
            public float VerticalOffset;
            public float ScaleOffset;

            private void Execute(Entity entity, [ChunkIndexInQuery] int chunkIndex, ref LocalTransform transform,
                in DamageBubble bubble)
            {
                var timeAlive = ElapsedTime - bubble.SpawnTime;
                if (timeAlive > lifeTime) ECBWriter.DestroyEntity(chunkIndex, entity);

                var easing = EaseOutQuad(timeAlive / lifeTime);
                transform.Position.y = bubble.OriginalY + VerticalOffset * easing;
                transform.Position.z += DeltaTime / 100;
                transform.Scale = 1 + ScaleOffset * easing;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static float EaseOutQuad(float number)
            {
                return 1 - (1 - number) * (1 - number);
            }
        }
    }
}