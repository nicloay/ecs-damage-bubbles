using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace DamageInfo
{
    public partial struct DamageBubbleMovementSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

            new MoveJob
            {
                ElapsedTime = (float)SystemAPI.Time.ElapsedTime,
                DeltaTime = SystemAPI.Time.DeltaTime,
                ECBWriter = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }

        [BurstCompile]
        public partial struct MoveJob : IJobEntity
        {
            public float ElapsedTime;
            public float DeltaTime;
            public EntityCommandBuffer.ParallelWriter ECBWriter;


            private void Execute(Entity entity, [ChunkIndexInQuery] int chunkIndex, ref LocalTransform transform,
                in DamageBubble bubble)
            {
                var timeAlive = ElapsedTime - bubble.SpawnTime;
                if (timeAlive > 1.5) ECBWriter.DestroyEntity(chunkIndex, entity);

                transform.Position.y += DeltaTime;
                transform.Position.z += DeltaTime / 100;
            }
        }
    }
}